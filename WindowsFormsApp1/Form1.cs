using WindowsFormsApp1.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click( object sender, EventArgs e )
        {
            //OpenFileDialog jsonFile = new OpenFileDialog();
            //jsonFile.Filter = "Json | *.json";
            //jsonFile.Title = "Select json file";
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "Select folder with json files";
            if( folder.ShowDialog() == DialogResult.OK ) {

                string newJsonPath = CreateJsonDirectory( folder.SelectedPath );
                List<HashDB> dBs = new List<HashDB>();
                var jsonFiles = Directory.GetFiles( folder.SelectedPath, "*.json" );
                foreach( string jsonPath in jsonFiles ) {
                    string fileName = jsonPath.Replace( folder.SelectedPath + "\\", "" );
                    fileName = fileName.Replace( ".json", "" );
                    JsonDb jsonDb = DeserializeJson( jsonPath );
                    if( jsonDb.ElementsList != null ) {
                        OdooModel odoo = new OdooModel( fileName, fileName, "equipment" );
                        foreach( ElementData elementData in jsonDb.ElementsList ) {
                            if( elementData.asset_format == "None" ) {
                                odoo.childObjects.Add( new ElementModel( elementData.Id, elementData.name, elementData.name, elementData.name,
                                    elementData.local_position, elementData.isMirrored, elementData.local_rotation, elementData.material ) );
                            }
                            else if( elementData.asset_format == ".json" ) {
                                odoo.childObjects.Add( new ElementModel( elementData.Id, elementData.name, elementData.name, elementData.name,
                                    elementData.local_position, elementData.isMirrored, elementData.local_rotation, elementData.material ) );
                            }
                            else {
                                odoo.childObjects.Add( new ElementModel( elementData.Id, elementData.name, elementData.name,"none",
                                    elementData.local_position, elementData.isMirrored, elementData.local_rotation, elementData.material ) );
                            }
                        }
                        var hashName = GetHashValue( fileName );
                        string resultFileName = $"{newJsonPath}\\{hashName}.json";
                        dBs.Add( new HashDB() { original_Name = fileName, hash_name = hashName } );
                        Serialize( odoo, resultFileName );
                    }
                }
                WriteHashName( dBs );
            }
        }

        private void WriteHashName(List<HashDB> dBs)
        {
            string documentsPath = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) + "\\HashCodeDB.json";
            if( !File.Exists( documentsPath ) ) {
                Serialize( dBs, documentsPath );
            }
            else {
                List<HashDB> hashDB = DeserializeHash( documentsPath );
                foreach( HashDB dB in dBs ) {
                    hashDB.Add( dB );
                }
                Serialize( hashDB, documentsPath );
            }
        }

        private void Serialize( List<HashDB> dbs, string path )
        {
            if( dbs is null )
                return;
            if( string.IsNullOrEmpty( path ) )
                return;
            var json = JsonConvert.SerializeObject( dbs );
            File.WriteAllText( path, json );
        }

        private string GetHashValue( string pass )
        {
            byte[] textToBytes = Encoding.UTF8.GetBytes( pass );
            SHA256Managed sha256 = new SHA256Managed();

            byte[] hasValue = sha256.ComputeHash( textToBytes );

            string fullHash = GetHexStringFromHash( hasValue );
            return fullHash.Substring( 0, 15 );
        }

        private string GetHexStringFromHash( byte[] hash )
        {
            string hexString = string.Empty;

            foreach( byte b in hash ) {
                hexString += b.ToString( "x2" );
            }

            return hexString;
        }

        private string CreateJsonDirectory(string folderPath)
        {
            DirectoryInfo result = Directory.CreateDirectory( $"{folderPath}\\JsonFiles" );
            return result.FullName;
        }

        private void Serialize( OdooModel jsonDb, string path )
        {
            if( jsonDb is null )
                return;
            if( string.IsNullOrEmpty( path ) )
                return;
            var json = JsonConvert.SerializeObject( jsonDb );
            File.WriteAllText( path, json );
        }

        public JsonDb DeserializeJson( string path )
        {
            if( !File.Exists( path ) ) {
                File.Create( path );
                return new JsonDb();
            }
            var json = File.ReadAllText( path );
            var jsonDb = JsonConvert.DeserializeObject<JsonDb>( json );
            return jsonDb ?? new JsonDb();
        }

        public List<HashDB> DeserializeHash( string path )
        {
            if( !File.Exists( path ) ) {
                File.Create( path );
                return new List<HashDB>();
            }
            var json = File.ReadAllText( path );
            var jsonDb = JsonConvert.DeserializeObject<List<HashDB>>( json );
            return jsonDb ?? new List<HashDB>();
        }
    }
}
