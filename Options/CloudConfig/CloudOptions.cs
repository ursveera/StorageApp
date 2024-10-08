﻿
namespace StorageApp.CloudProvider.Config
{ 
    public class CloudOptions
    {
        public AWSOptions AWS { get; set; }
        public AZUREOptions Azure { get; set; }
        public GCPOptions Gcp { get; set; }
        public NOCLOUDOptions NoCloud { get; set; }
        public string Target { get; set; }
    }
}
