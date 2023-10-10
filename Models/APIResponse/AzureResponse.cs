using System.Xml.Serialization;

namespace StorageApp.Models.APIResponse
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Serializable, XmlRoot("EnumerationResults")]
    public partial class AzureResponse
    {

        private EnumerationResultsBlob[] blobsField;

        private object nextMarkerField;

        private string serviceEndpointField;

        private string containerNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Blob", IsNullable = false)]
        public EnumerationResultsBlob[] Blobs
        {
            get
            {
                return this.blobsField;
            }
            set
            {
                this.blobsField = value;
            }
        }

        /// <remarks/>
        public object NextMarker
        {
            get
            {
                return this.nextMarkerField;
            }
            set
            {
                this.nextMarkerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ServiceEndpoint
        {
            get
            {
                return this.serviceEndpointField;
            }
            set
            {
                this.serviceEndpointField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ContainerName
        {
            get
            {
                return this.containerNameField;
            }
            set
            {
                this.containerNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class EnumerationResultsBlob
    {

        private string nameField;

        private EnumerationResultsBlobProperties propertiesField;

        private object orMetadataField;

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
        public EnumerationResultsBlobProperties Properties
        {
            get
            {
                return this.propertiesField;
            }
            set
            {
                this.propertiesField = value;
            }
        }

        /// <remarks/>
        public object OrMetadata
        {
            get
            {
                return this.orMetadataField;
            }
            set
            {
                this.orMetadataField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class EnumerationResultsBlobProperties
    {

        private string creationTimeField;

        private string lastModifiedField;

        private string etagField;

        private ushort contentLengthField;

        private string contentTypeField;

        private object contentEncodingField;

        private object contentLanguageField;

        private object contentCRC64Field;

        private string contentMD5Field;

        private object cacheControlField;

        private object contentDispositionField;

        private string blobTypeField;

        private string accessTierField;

        private bool accessTierInferredField;

        private string leaseStatusField;

        private string leaseStateField;

        private bool serverEncryptedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Creation-Time")]
        public string CreationTime
        {
            get
            {
                return this.creationTimeField;
            }
            set
            {
                this.creationTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Last-Modified")]
        public string LastModified
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
        public string Etag
        {
            get
            {
                return this.etagField;
            }
            set
            {
                this.etagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Content-Length")]
        public ushort ContentLength
        {
            get
            {
                return this.contentLengthField;
            }
            set
            {
                this.contentLengthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Content-Type")]
        public string ContentType
        {
            get
            {
                return this.contentTypeField;
            }
            set
            {
                this.contentTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Content-Encoding")]
        public object ContentEncoding
        {
            get
            {
                return this.contentEncodingField;
            }
            set
            {
                this.contentEncodingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Content-Language")]
        public object ContentLanguage
        {
            get
            {
                return this.contentLanguageField;
            }
            set
            {
                this.contentLanguageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Content-CRC64")]
        public object ContentCRC64
        {
            get
            {
                return this.contentCRC64Field;
            }
            set
            {
                this.contentCRC64Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Content-MD5")]
        public string ContentMD5
        {
            get
            {
                return this.contentMD5Field;
            }
            set
            {
                this.contentMD5Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Cache-Control")]
        public object CacheControl
        {
            get
            {
                return this.cacheControlField;
            }
            set
            {
                this.cacheControlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Content-Disposition")]
        public object ContentDisposition
        {
            get
            {
                return this.contentDispositionField;
            }
            set
            {
                this.contentDispositionField = value;
            }
        }

        /// <remarks/>
        public string BlobType
        {
            get
            {
                return this.blobTypeField;
            }
            set
            {
                this.blobTypeField = value;
            }
        }

        /// <remarks/>
        public string AccessTier
        {
            get
            {
                return this.accessTierField;
            }
            set
            {
                this.accessTierField = value;
            }
        }

        /// <remarks/>
        public bool AccessTierInferred
        {
            get
            {
                return this.accessTierInferredField;
            }
            set
            {
                this.accessTierInferredField = value;
            }
        }

        /// <remarks/>
        public string LeaseStatus
        {
            get
            {
                return this.leaseStatusField;
            }
            set
            {
                this.leaseStatusField = value;
            }
        }

        /// <remarks/>
        public string LeaseState
        {
            get
            {
                return this.leaseStateField;
            }
            set
            {
                this.leaseStateField = value;
            }
        }

        /// <remarks/>
        public bool ServerEncrypted
        {
            get
            {
                return this.serverEncryptedField;
            }
            set
            {
                this.serverEncryptedField = value;
            }
        }
    }


}
