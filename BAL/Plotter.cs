using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Drawing;
using System.IO;

namespace BAL
{

    public class Plotter
    {

        #region VARIABLES

        //list of points for the current dotSignFile-part
        private List<Point> m_listPoints;
        //comma-delimited x,y co-ordinates for all the points of signature
        private StringBuilder m_strPoints = new StringBuilder();
        //grafix canvas of the form which uses this plotter
        private Graphics m_gfxCanvas;
        //ractangular area which m_gfxCanvas uses for its clip region
        private Rectangle m_rectClip;
        //background colorizing tool for the m_gfxCanvas        
        static private Brush m_brushBg = new SolidBrush(Color.White);
        //pen used to draw points as lines
        static private Pen m_penLine = new Pen(Color.Blue);

        #endregion

        #region BASIC DRAWING CODE

        //the only constructor for the calling form

        //params: 

        //Graphics gfx: the caller form's grafix
        //int x, int y: the drawing canvas location co-ordintaes
        //int width, int height: the drawing canvas dimension
        public Plotter(Graphics gfx, int x, int y, int width, int height)
        {
            try
            {
                //initialize the plotter's members
                m_listPoints = new List<Point>();
                m_gfxCanvas = gfx;
                m_rectClip = new Rectangle(x, y, width, height);
                m_gfxCanvas.Clip = new Region(m_rectClip);
                //draw the rectangular canvas as the plotter's signature pad
                m_gfxCanvas.FillRectangle(m_brushBg, m_rectClip);
            }
            catch (Exception)
            {
            }
        }

        //the main plotter which adds up all the points sent by FrmSignPad_MouseMove; 
        //and draws all the currently held points in m_listPoints onto the canvas
        public void ReDraw(Point aPoint)
        {
            try
            {
                //this check is to get rid of a runtime bug: 
                //as the user play with <Options> menuitem, that mouse location is stored as the first/starting point
                //which we must ignore; comment out this condition to see what happens
                if (aPoint.X > this.m_rectClip.Left && aPoint.Y > this.m_rectClip.Top)
                    m_listPoints.Add(aPoint);

                Draw();
            }
            catch (Exception)
            {
            }
        }

        //called by ReDraw AND the load functionality given below 
        public void Draw()
        {
            try
            {
                float xTo = 0;
                float xFrom = 0;
                float yTo = 0;
                float yFrom = 0;

                if (m_listPoints.Count < 2)//can't draw 1 point only; coz, only lines are drawn here
                    return;

                for (int i = 1; i < m_listPoints.Count; i++)
                {
                    //co-ordinates of starting point
                    xFrom = m_listPoints[i - 1].X;
                    yFrom = m_listPoints[i - 1].Y;
                    //co-ordinates of ending point
                    xTo = m_listPoints[i].X;
                    yTo = m_listPoints[i].Y;
                    //draw a line segment
                    m_gfxCanvas.DrawLine(m_penLine, (int)xTo, (int)yTo, (int)xFrom, (int)yFrom);

                }
            }
            catch (Exception)
            {
            }
        }

        //called by FrmSign_btnClear_Click: re-draws the m_gfxCanvas;also initializes everything else
        public void Clear()
        {
            //create a new grafix canvas layer to be used for new dotSignFile. drawing
            m_gfxCanvas.FillRectangle(m_brushBg, m_rectClip);
            m_listPoints.Clear();
            m_strPoints.Remove(0, this.m_strPoints.Length);//StringBuilder's way of clear    
        }

        public void DrawRectangle()
        {
            //create a new grafix canvas layer to be used for new dotSignFile. drawing
            m_gfxCanvas.FillRectangle(m_brushBg, m_rectClip);
        }

        #endregion

        #region ADDITIONAL SAVE/LOAD FUNCTIONALITY

        //appends currently held points to the string of all points (i.e. m_strPoints) and clears the cashe (m_listPoints)
        //called when mouse up OR when one dotSignFile-part is to be stored as string        
        public void SaveAndClearPoints()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < m_listPoints.Count; i++)
                {
                    sb.Append(m_listPoints[i].X + "," + m_listPoints[i].Y + ",");
                }
                string strCoordinates;            
                if (sb.Length > 0)
                {
                    strCoordinates = sb.ToString().Substring(0, sb.ToString().Length - 1);//trim of last ','        
                    this.m_strPoints.Append(strCoordinates + ";");//all dotSignFile-parts would be separated by ';', e.g. 12,334,13,34;122,23,34,45;etc...
                }             
                //when done, clear the list and for the next points' addition (i.e. FrmSignPad_MouseMove results) 
                m_listPoints.Clear();
            }
            catch (Exception)
            {
            }
        }

        //read-only property to make up list of all drawn points 
        //which were stored in a string buffer (m_strPoints) as a linear sequence of comma-delimted x-y co-ordinates        
        public List<Point> Points
        {
            get
            {
                List<Point> points = new List<Point>(); //empty bag
                string strPoints = this.ToString();//all dotSignFile-parts                
                points = Plotter.ToPoints(strPoints);
                return points;
            }
        }

        // a plotter's toString() returns all the points drawn till now: e.g 12,123;153,23 etc...
        public override string ToString()
        {

            string points = this.m_strPoints.ToString();

            if (points.Length > 0)
                return points.Substring(0, points.Length - 1);//to remove trailing ';'    
            else
                return points;

        }

        //creates a continuous list of points of the parameter signString
        //a worker method for this.Points
        public static List<Point> ToPoints(string signString)
        {
            string[] signParts = signString.Split(';');//collect all dotSignFile-parts as strings
            List<Point> seriesOfPoints = new List<Point>();
            foreach (string signPart in signParts)
            {

                string[] arrPoints = signPart.Split(',');//collect all x\y numbers as strings
                for (int i = 1; i < arrPoints.Length; )
                {
                    string strX = arrPoints[i - 1];
                    string strY = arrPoints[i];
                    i = i + 2;
                    Point p = new Point(Convert.ToInt32(strX), Convert.ToInt32(strY));
                    seriesOfPoints.Add(p);
                }
            }
            return seriesOfPoints;
        }

        //gets single-line string data from a .dotSignFile file
        public void OpenSign(string file)
        {
            StreamReader sr = new StreamReader(file);
            string signString = "";
            if (!sr.EndOfStream)
                signString = sr.ReadLine();
            if (signString != "")
                ReDraw(signString);
            sr.Close();

        }

        //a worker method for OpenSign(string file)
        //draws the saved signature on the canvas
        public void ReDraw(string signString)
        {
            this.Clear();//dotSignFile pad must have no pending or old drawing

            string[] signParts_asStr = signString.Split(';');//collect all dotSignFile-parts as strings

            foreach (string signPart in signParts_asStr)
            {
                string[] arrPoints = signPart.Split(',');//collect all x\y numbers as strings
                for (int i = 1; i < arrPoints.Length; )
                {
                    string strX = arrPoints[i - 1];
                    string strY = arrPoints[i];
                    i = i + 2;
                    Point p = new Point(Convert.ToInt32(strX), Convert.ToInt32(strY));
                    this.m_listPoints.Add(p);
                }
                // done with this signPart? draw it! store it! and clear the cashe for next signPart
                Draw();
                SaveAndClearPoints();

            }// do the same for next signPart if any

            //signature is re-drawn and ready for edit
        }

        //////persists the current drawing in a file as a single-line string
        ////public void SaveSign(string file)
        ////{
        ////    StreamWriter sw = new StreamWriter(file);
        ////    sw.WriteLine(this.ToString());
        ////    sw.Flush();
        ////    sw.Close();
        ////}

        //persists the current drawing in a file as a single-line string
        public string SaveSign()
        {
            return (this.ToString());
        }

        #endregion

    }

}
