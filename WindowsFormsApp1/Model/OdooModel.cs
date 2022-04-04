using Newtonsoft.Json;
using System.Collections.Generic;

namespace WindowsFormsApp1.Model
{
    [JsonObject]
    public class OdooModel
    {
        public string name;
        public string main_modelId;
        public string parent_inventoryId;
        public string objectType;
        public List<ElementModel> childObjects = new List<ElementModel>();
        public OdooModel( string name, string modelId, string objectType )
        {
            this.name = name;
            main_modelId = name;
            parent_inventoryId = "none";
            this.objectType = objectType;
        }
    }
}
