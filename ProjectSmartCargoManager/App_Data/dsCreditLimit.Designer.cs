﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5420
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 1591

namespace ProjectSmartCargoManager.App_Data {
    
    
    /// <summary>
    ///Represents a strongly typed in-memory cache of data.
    ///</summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    [global::System.Serializable()]
    [global::System.ComponentModel.DesignerCategoryAttribute("code")]
    [global::System.ComponentModel.ToolboxItem(true)]
    [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedDataSetSchema")]
    [global::System.Xml.Serialization.XmlRootAttribute("dsCreditLimit")]
    [global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.DataSet")]
    public partial class dsCreditLimit : global::System.Data.DataSet {
        
        private dtCreditLimitDataTable tabledtCreditLimit;
        
        private global::System.Data.SchemaSerializationMode _schemaSerializationMode = global::System.Data.SchemaSerializationMode.IncludeSchema;
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public dsCreditLimit() {
            this.BeginInit();
            this.InitClass();
            global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            base.Tables.CollectionChanged += schemaChangedHandler;
            base.Relations.CollectionChanged += schemaChangedHandler;
            this.EndInit();
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected dsCreditLimit(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) : 
                base(info, context, false) {
            if ((this.IsBinarySerialized(info, context) == true)) {
                this.InitVars(false);
                global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler1 = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
                this.Tables.CollectionChanged += schemaChangedHandler1;
                this.Relations.CollectionChanged += schemaChangedHandler1;
                return;
            }
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((this.DetermineSchemaSerializationMode(info, context) == global::System.Data.SchemaSerializationMode.IncludeSchema)) {
                global::System.Data.DataSet ds = new global::System.Data.DataSet();
                ds.ReadXmlSchema(new global::System.Xml.XmlTextReader(new global::System.IO.StringReader(strSchema)));
                if ((ds.Tables["dtCreditLimit"] != null)) {
                    base.Tables.Add(new dtCreditLimitDataTable(ds.Tables["dtCreditLimit"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, global::System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.ReadXmlSchema(new global::System.Xml.XmlTextReader(new global::System.IO.StringReader(strSchema)));
            }
            this.GetSerializationData(info, context);
            global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            base.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.Browsable(false)]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        public dtCreditLimitDataTable dtCreditLimit {
            get {
                return this.tabledtCreditLimit;
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.BrowsableAttribute(true)]
        [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public override global::System.Data.SchemaSerializationMode SchemaSerializationMode {
            get {
                return this._schemaSerializationMode;
            }
            set {
                this._schemaSerializationMode = value;
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public new global::System.Data.DataTableCollection Tables {
            get {
                return base.Tables;
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public new global::System.Data.DataRelationCollection Relations {
            get {
                return base.Relations;
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override void InitializeDerivedDataSet() {
            this.BeginInit();
            this.InitClass();
            this.EndInit();
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public override global::System.Data.DataSet Clone() {
            dsCreditLimit cln = ((dsCreditLimit)(base.Clone()));
            cln.InitVars();
            cln.SchemaSerializationMode = this.SchemaSerializationMode;
            return cln;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override void ReadXmlSerializable(global::System.Xml.XmlReader reader) {
            if ((this.DetermineSchemaSerializationMode(reader) == global::System.Data.SchemaSerializationMode.IncludeSchema)) {
                this.Reset();
                global::System.Data.DataSet ds = new global::System.Data.DataSet();
                ds.ReadXml(reader);
                if ((ds.Tables["dtCreditLimit"] != null)) {
                    base.Tables.Add(new dtCreditLimitDataTable(ds.Tables["dtCreditLimit"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, global::System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.ReadXml(reader);
                this.InitVars();
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override global::System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            global::System.IO.MemoryStream stream = new global::System.IO.MemoryStream();
            this.WriteXmlSchema(new global::System.Xml.XmlTextWriter(stream, null));
            stream.Position = 0;
            return global::System.Xml.Schema.XmlSchema.Read(new global::System.Xml.XmlTextReader(stream), null);
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal void InitVars() {
            this.InitVars(true);
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal void InitVars(bool initTable) {
            this.tabledtCreditLimit = ((dtCreditLimitDataTable)(base.Tables["dtCreditLimit"]));
            if ((initTable == true)) {
                if ((this.tabledtCreditLimit != null)) {
                    this.tabledtCreditLimit.InitVars();
                }
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void InitClass() {
            this.DataSetName = "dsCreditLimit";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/dsCreditLimit.xsd";
            this.EnforceConstraints = true;
            this.SchemaSerializationMode = global::System.Data.SchemaSerializationMode.IncludeSchema;
            this.tabledtCreditLimit = new dtCreditLimitDataTable();
            base.Tables.Add(this.tabledtCreditLimit);
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private bool ShouldSerializedtCreditLimit() {
            return false;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void SchemaChanged(object sender, global::System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == global::System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedDataSetSchema(global::System.Xml.Schema.XmlSchemaSet xs) {
            dsCreditLimit ds = new dsCreditLimit();
            global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
            global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
            global::System.Xml.Schema.XmlSchemaAny any = new global::System.Xml.Schema.XmlSchemaAny();
            any.Namespace = ds.Namespace;
            sequence.Items.Add(any);
            type.Particle = sequence;
            global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
            if (xs.Contains(dsSchema.TargetNamespace)) {
                global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                try {
                    global::System.Xml.Schema.XmlSchema schema = null;
                    dsSchema.Write(s1);
                    for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); ) {
                        schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                        s2.SetLength(0);
                        schema.Write(s2);
                        if ((s1.Length == s2.Length)) {
                            s1.Position = 0;
                            s2.Position = 0;
                            for (; ((s1.Position != s1.Length) 
                                        && (s1.ReadByte() == s2.ReadByte())); ) {
                                ;
                            }
                            if ((s1.Position == s1.Length)) {
                                return type;
                            }
                        }
                    }
                }
                finally {
                    if ((s1 != null)) {
                        s1.Close();
                    }
                    if ((s2 != null)) {
                        s2.Close();
                    }
                }
            }
            xs.Add(dsSchema);
            return type;
        }
        
        public delegate void dtCreditLimitRowChangeEventHandler(object sender, dtCreditLimitRowChangeEvent e);
        
        /// <summary>
        ///Represents the strongly named DataTable class.
        ///</summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        [global::System.Serializable()]
        [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
        public partial class dtCreditLimitDataTable : global::System.Data.TypedTableBase<dtCreditLimitRow> {
            
            private global::System.Data.DataColumn columnAgent;
            
            private global::System.Data.DataColumn columnAgentName;
            
            private global::System.Data.DataColumn columnCreditLimit;
            
            private global::System.Data.DataColumn columnInvoiceBalance;
            
            private global::System.Data.DataColumn columnCreditAmount;
            
            private global::System.Data.DataColumn columnCreditRemaining;
            
            private global::System.Data.DataColumn columnLogo;
            
            private global::System.Data.DataColumn columnAgent1;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public dtCreditLimitDataTable() {
                this.TableName = "dtCreditLimit";
                this.BeginInit();
                this.InitClass();
                this.EndInit();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal dtCreditLimitDataTable(global::System.Data.DataTable table) {
                this.TableName = table.TableName;
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected dtCreditLimitDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) : 
                    base(info, context) {
                this.InitVars();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn AgentColumn {
                get {
                    return this.columnAgent;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn AgentNameColumn {
                get {
                    return this.columnAgentName;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn CreditLimitColumn {
                get {
                    return this.columnCreditLimit;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn InvoiceBalanceColumn {
                get {
                    return this.columnInvoiceBalance;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn CreditAmountColumn {
                get {
                    return this.columnCreditAmount;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn CreditRemainingColumn {
                get {
                    return this.columnCreditRemaining;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn LogoColumn {
                get {
                    return this.columnLogo;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn Agent1Column {
                get {
                    return this.columnAgent1;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public dtCreditLimitRow this[int index] {
                get {
                    return ((dtCreditLimitRow)(this.Rows[index]));
                }
            }
            
            public event dtCreditLimitRowChangeEventHandler dtCreditLimitRowChanging;
            
            public event dtCreditLimitRowChangeEventHandler dtCreditLimitRowChanged;
            
            public event dtCreditLimitRowChangeEventHandler dtCreditLimitRowDeleting;
            
            public event dtCreditLimitRowChangeEventHandler dtCreditLimitRowDeleted;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void AdddtCreditLimitRow(dtCreditLimitRow row) {
                this.Rows.Add(row);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public dtCreditLimitRow AdddtCreditLimitRow(string Agent, string AgentName, string CreditLimit, string InvoiceBalance, string CreditAmount, string CreditRemaining, byte[] Logo, string Agent1) {
                dtCreditLimitRow rowdtCreditLimitRow = ((dtCreditLimitRow)(this.NewRow()));
                object[] columnValuesArray = new object[] {
                        Agent,
                        AgentName,
                        CreditLimit,
                        InvoiceBalance,
                        CreditAmount,
                        CreditRemaining,
                        Logo,
                        Agent1};
                rowdtCreditLimitRow.ItemArray = columnValuesArray;
                this.Rows.Add(rowdtCreditLimitRow);
                return rowdtCreditLimitRow;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public override global::System.Data.DataTable Clone() {
                dtCreditLimitDataTable cln = ((dtCreditLimitDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override global::System.Data.DataTable CreateInstance() {
                return new dtCreditLimitDataTable();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal void InitVars() {
                this.columnAgent = base.Columns["Agent"];
                this.columnAgentName = base.Columns["AgentName"];
                this.columnCreditLimit = base.Columns["CreditLimit"];
                this.columnInvoiceBalance = base.Columns["InvoiceBalance"];
                this.columnCreditAmount = base.Columns["CreditAmount"];
                this.columnCreditRemaining = base.Columns["CreditRemaining"];
                this.columnLogo = base.Columns["Logo"];
                this.columnAgent1 = base.Columns["Agent1"];
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private void InitClass() {
                this.columnAgent = new global::System.Data.DataColumn("Agent", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnAgent);
                this.columnAgentName = new global::System.Data.DataColumn("AgentName", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnAgentName);
                this.columnCreditLimit = new global::System.Data.DataColumn("CreditLimit", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnCreditLimit);
                this.columnInvoiceBalance = new global::System.Data.DataColumn("InvoiceBalance", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnInvoiceBalance);
                this.columnCreditAmount = new global::System.Data.DataColumn("CreditAmount", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnCreditAmount);
                this.columnCreditRemaining = new global::System.Data.DataColumn("CreditRemaining", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnCreditRemaining);
                this.columnLogo = new global::System.Data.DataColumn("Logo", typeof(byte[]), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnLogo);
                this.columnAgent1 = new global::System.Data.DataColumn("Agent1", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnAgent1);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public dtCreditLimitRow NewdtCreditLimitRow() {
                return ((dtCreditLimitRow)(this.NewRow()));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder) {
                return new dtCreditLimitRow(builder);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override global::System.Type GetRowType() {
                return typeof(dtCreditLimitRow);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.dtCreditLimitRowChanged != null)) {
                    this.dtCreditLimitRowChanged(this, new dtCreditLimitRowChangeEvent(((dtCreditLimitRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.dtCreditLimitRowChanging != null)) {
                    this.dtCreditLimitRowChanging(this, new dtCreditLimitRowChangeEvent(((dtCreditLimitRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.dtCreditLimitRowDeleted != null)) {
                    this.dtCreditLimitRowDeleted(this, new dtCreditLimitRowChangeEvent(((dtCreditLimitRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.dtCreditLimitRowDeleting != null)) {
                    this.dtCreditLimitRowDeleting(this, new dtCreditLimitRowChangeEvent(((dtCreditLimitRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void RemovedtCreditLimitRow(dtCreditLimitRow row) {
                this.Rows.Remove(row);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs) {
                global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                dsCreditLimit ds = new dsCreditLimit();
                global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                any1.MinOccurs = new decimal(0);
                any1.MaxOccurs = decimal.MaxValue;
                any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                sequence.Items.Add(any1);
                global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                any2.MinOccurs = new decimal(1);
                any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                sequence.Items.Add(any2);
                global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                attribute1.Name = "namespace";
                attribute1.FixedValue = ds.Namespace;
                type.Attributes.Add(attribute1);
                global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                attribute2.Name = "tableTypeName";
                attribute2.FixedValue = "dtCreditLimitDataTable";
                type.Attributes.Add(attribute2);
                type.Particle = sequence;
                global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                if (xs.Contains(dsSchema.TargetNamespace)) {
                    global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                    global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                    try {
                        global::System.Xml.Schema.XmlSchema schema = null;
                        dsSchema.Write(s1);
                        for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); ) {
                            schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                            s2.SetLength(0);
                            schema.Write(s2);
                            if ((s1.Length == s2.Length)) {
                                s1.Position = 0;
                                s2.Position = 0;
                                for (; ((s1.Position != s1.Length) 
                                            && (s1.ReadByte() == s2.ReadByte())); ) {
                                    ;
                                }
                                if ((s1.Position == s1.Length)) {
                                    return type;
                                }
                            }
                        }
                    }
                    finally {
                        if ((s1 != null)) {
                            s1.Close();
                        }
                        if ((s2 != null)) {
                            s2.Close();
                        }
                    }
                }
                xs.Add(dsSchema);
                return type;
            }
        }
        
        /// <summary>
        ///Represents strongly named DataRow class.
        ///</summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        public partial class dtCreditLimitRow : global::System.Data.DataRow {
            
            private dtCreditLimitDataTable tabledtCreditLimit;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal dtCreditLimitRow(global::System.Data.DataRowBuilder rb) : 
                    base(rb) {
                this.tabledtCreditLimit = ((dtCreditLimitDataTable)(this.Table));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string Agent {
                get {
                    try {
                        return ((string)(this[this.tabledtCreditLimit.AgentColumn]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("The value for column \'Agent\' in table \'dtCreditLimit\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tabledtCreditLimit.AgentColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string AgentName {
                get {
                    try {
                        return ((string)(this[this.tabledtCreditLimit.AgentNameColumn]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("The value for column \'AgentName\' in table \'dtCreditLimit\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tabledtCreditLimit.AgentNameColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string CreditLimit {
                get {
                    try {
                        return ((string)(this[this.tabledtCreditLimit.CreditLimitColumn]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("The value for column \'CreditLimit\' in table \'dtCreditLimit\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tabledtCreditLimit.CreditLimitColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string InvoiceBalance {
                get {
                    try {
                        return ((string)(this[this.tabledtCreditLimit.InvoiceBalanceColumn]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("The value for column \'InvoiceBalance\' in table \'dtCreditLimit\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tabledtCreditLimit.InvoiceBalanceColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string CreditAmount {
                get {
                    try {
                        return ((string)(this[this.tabledtCreditLimit.CreditAmountColumn]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("The value for column \'CreditAmount\' in table \'dtCreditLimit\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tabledtCreditLimit.CreditAmountColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string CreditRemaining {
                get {
                    try {
                        return ((string)(this[this.tabledtCreditLimit.CreditRemainingColumn]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("The value for column \'CreditRemaining\' in table \'dtCreditLimit\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tabledtCreditLimit.CreditRemainingColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public byte[] Logo {
                get {
                    try {
                        return ((byte[])(this[this.tabledtCreditLimit.LogoColumn]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("The value for column \'Logo\' in table \'dtCreditLimit\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tabledtCreditLimit.LogoColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string Agent1 {
                get {
                    try {
                        return ((string)(this[this.tabledtCreditLimit.Agent1Column]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("The value for column \'Agent1\' in table \'dtCreditLimit\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tabledtCreditLimit.Agent1Column] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsAgentNull() {
                return this.IsNull(this.tabledtCreditLimit.AgentColumn);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetAgentNull() {
                this[this.tabledtCreditLimit.AgentColumn] = global::System.Convert.DBNull;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsAgentNameNull() {
                return this.IsNull(this.tabledtCreditLimit.AgentNameColumn);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetAgentNameNull() {
                this[this.tabledtCreditLimit.AgentNameColumn] = global::System.Convert.DBNull;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsCreditLimitNull() {
                return this.IsNull(this.tabledtCreditLimit.CreditLimitColumn);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetCreditLimitNull() {
                this[this.tabledtCreditLimit.CreditLimitColumn] = global::System.Convert.DBNull;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsInvoiceBalanceNull() {
                return this.IsNull(this.tabledtCreditLimit.InvoiceBalanceColumn);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetInvoiceBalanceNull() {
                this[this.tabledtCreditLimit.InvoiceBalanceColumn] = global::System.Convert.DBNull;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsCreditAmountNull() {
                return this.IsNull(this.tabledtCreditLimit.CreditAmountColumn);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetCreditAmountNull() {
                this[this.tabledtCreditLimit.CreditAmountColumn] = global::System.Convert.DBNull;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsCreditRemainingNull() {
                return this.IsNull(this.tabledtCreditLimit.CreditRemainingColumn);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetCreditRemainingNull() {
                this[this.tabledtCreditLimit.CreditRemainingColumn] = global::System.Convert.DBNull;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsLogoNull() {
                return this.IsNull(this.tabledtCreditLimit.LogoColumn);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetLogoNull() {
                this[this.tabledtCreditLimit.LogoColumn] = global::System.Convert.DBNull;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsAgent1Null() {
                return this.IsNull(this.tabledtCreditLimit.Agent1Column);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetAgent1Null() {
                this[this.tabledtCreditLimit.Agent1Column] = global::System.Convert.DBNull;
            }
        }
        
        /// <summary>
        ///Row event argument class
        ///</summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        public class dtCreditLimitRowChangeEvent : global::System.EventArgs {
            
            private dtCreditLimitRow eventRow;
            
            private global::System.Data.DataRowAction eventAction;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public dtCreditLimitRowChangeEvent(dtCreditLimitRow row, global::System.Data.DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public dtCreditLimitRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}

#pragma warning restore 1591