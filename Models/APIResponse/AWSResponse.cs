using System.Xml.Serialization;

namespace StorageApp.Models.APIResponse
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://s3.amazonaws.com/doc/2006-03-01/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://s3.amazonaws.com/doc/2006-03-01/", IsNullable = false)]
    public partial class ListBucketResult
    {

        private string nameField;

        private object prefixField;

        private object markerField;

        private ushort maxKeysField;

        private bool isTruncatedField;

        private ListBucketResultContents[] contentsField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public object Prefix
        {
            get
            {
                return this.prefixField;
            }
            set
            {
                this.prefixField = value;
            }
        }

        /// <remarks/>
        public object Marker
        {
            get
            {
                return this.markerField;
            }
            set
            {
                this.markerField = value;
            }
        }

        /// <remarks/>
        public ushort MaxKeys
        {
            get
            {
                return this.maxKeysField;
            }
            set
            {
                this.maxKeysField = value;
            }
        }

        /// <remarks/>
        public bool IsTruncated
        {
            get
            {
                return this.isTruncatedField;
            }
            set
            {
                this.isTruncatedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Contents")]
        public ListBucketResultContents[] Contents
        {
            get
            {
                return this.contentsField;
            }
            set
            {
                this.contentsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://s3.amazonaws.com/doc/2006-03-01/")]
    public partial class ListBucketResultContents
    {

        private string keyField;

        private System.DateTime lastModifiedField;

        private string eTagField;

        private uint sizeField;

        private ListBucketResultContentsOwner ownerField;

        private string storageClassField;

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        public System.DateTime LastModified
        {
            get
            {
                return this.lastModifiedField;
            }
            set
            {
                this.lastModifiedField = value;
            }
        }

        /// <remarks/>
        public string ETag
        {
            get
            {
                return this.eTagField;
            }
            set
            {
                this.eTagField = value;
            }
        }

        /// <remarks/>
        public uint Size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }

        /// <remarks/>
        public ListBucketResultContentsOwner Owner
        {
            get
            {
                return this.ownerField;
            }
            set
            {
                this.ownerField = value;
            }
        }

        /// <remarks/>
        public string StorageClass
        {
            get
            {
                return this.storageClassField;
            }
            set
            {
                this.storageClassField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://s3.amazonaws.com/doc/2006-03-01/")]
    public partial class ListBucketResultContentsOwner
    {

        private string idField;

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }


}
