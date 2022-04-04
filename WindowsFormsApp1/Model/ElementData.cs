using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WindowsFormsApp1.Model
{
    [JsonObject]
    public class ElementData
    {
        public string Id;
        public string name;
        public string asset_id;
        public string inventory_number;
        public string asset_format;
        public Position local_position = new Position(0,0,0);
        public Position local_rotation = new Position(0,0,0);
        public ElementCurveLocation curve_location = new ElementCurveLocation();
        public List<ElementParameter> parameters = new List<ElementParameter>();
        public List<ElementMaterial> material;
        public bool isMirrored;
        public List<ElementData> child_elements = new List<ElementData>();
    }

    [Serializable]
    public class JsonDb
    {
        public List<ElementData> ElementsList = new List<ElementData>();
    }
}
