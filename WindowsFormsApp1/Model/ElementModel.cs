using Newtonsoft.Json;
using System.Collections.Generic;

namespace WindowsFormsApp1.Model
{
    [JsonObject]
    public class ElementModel
    {
        public string revit_id;// id из Revit
        public string name; // element name from Revit
        public string objectId; // data from odoo
        public string model_id; // parameter in element from Revit
        public string inventoryId; // none value 
        public string detailed_model_id; // none value

        public Position localPosition = new Position( 0, 0, 0 );
        public Position localRotation = new Position( 0, 0, 0 );
        public List<ElementParameter> parameters = new List<ElementParameter>();
        public List<ElementMaterial> material = new List<ElementMaterial>();

        public ElementModel( string element_id, string element_name, string model, string detailed, Position location, bool IsMirrored,
            Position rotation, List<ElementMaterial> materials )
        {
            revit_id = element_id;
            this.name = element_name;
            model_id = model;
            objectId = "none";
            inventoryId = "none";
            detailed_model_id = "none";
            localPosition = location;
            localRotation = rotation;
            this.material = materials;
            detailed_model_id = detailed;
            parameters.Add( new ElementParameter() { parameter_name = "IsMirrored", parameter_value = $"{IsMirrored}" } );
        }
    }
}   
