using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
//Using que fueron remplazados por privacidad
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace NombreNoReal
{
    [Authorize]
    public class GeneradorCodigoController : BaseController
    {
 
        private IAccountService accountService;
        private IGeneradorCodigoService codeDataApi;
        private string errorBarCode = "";
 
        public GeneradorCodigoController()
            : this(new GeneradorCodigoService(), new AccountService())
        {

        }

        public GeneradorCodigoController(IGeneradorCodigoService _codeDataApi, IAccountService _accountService)
        {
            this.accountService = _accountService;
            this.codeDataApi = _codeDataApi;
        }
        public ActionResult Index(CodigoDeBarras data)
        {
             
            data.TipoCodes = codeDataApi.GetTypesBarCode().ToList();
            //return View();
            return View("Index", data);
        }


        public ActionResult Generar(string GTIN, string TIPO, int ancho, int alto,int rotate, string formato)
        {
            CodigoDeBarras barcode = GenerarCodigo(GTIN, TIPO, ancho, alto,rotate, formato);
            barcode.TipoCodes = codeDataApi.GetTypesBarCode().ToList();
            return View("Index", barcode);
        }

        public static bool IsValidGtin(string code)
        {
            if (code.Length <= 13)
            {


                //if (code != (new Regex("[^0-9]")).Replace(code, ""))
                //{
                //    // is not numeric
                //    return false;
                //}
                // pad with zeros to lengthen to 14 digits
                switch (code.Length)
                {
                    case 8:
                        code = "000000" + code;
                        break;
                    case 12:
                        code = "00" + code;
                        break;
                    case 13:
                        code = "0" + code;
                        break;
                    case 14:
                        break;
                    default:
                        // wrong number of digits
                        return false;
                }
                // calculate check digit
                int[] a = new int[13];
                a[0] = int.Parse(code[0].ToString()) * 3;
                a[1] = int.Parse(code[1].ToString());
                a[2] = int.Parse(code[2].ToString()) * 3;
                a[3] = int.Parse(code[3].ToString());
                a[4] = int.Parse(code[4].ToString()) * 3;
                a[5] = int.Parse(code[5].ToString());
                a[6] = int.Parse(code[6].ToString()) * 3;
                a[7] = int.Parse(code[7].ToString());
                a[8] = int.Parse(code[8].ToString()) * 3;
                a[9] = int.Parse(code[9].ToString());
                a[10] = int.Parse(code[10].ToString()) * 3;
                a[11] = int.Parse(code[11].ToString());
                a[12] = int.Parse(code[12].ToString()) * 3;
                int sum = a[0] + a[1] + a[2] + a[3] + a[4] + a[5] + a[6] + a[7] + a[8] + a[9] + a[10] + a[11] + a[12];
                int check = (10 - (sum % 10)) % 10;
                // evaluate check digit
                int last = int.Parse(code[13].ToString());
                return check == last;
            }
            return true;
        }

        private CodigoDeBarras GenerarCodigo(string GTIN, string TIPO, int ancho, int alto, int rotate, string formato)
        {
            CodigoDeBarras barcode = new CodigoDeBarras();
            barcode.errors = "";
            barcode.GTIN = GTIN;
            if(alto == null || alto == 0)
            {
                barcode.alto = "2";
            }
            if (ancho == null || ancho == 0)
            {
                barcode.ancho = "2";
            }

            if (rotate == null || rotate == 0)
            {
                barcode.rotate = "0";
            }
            barcode.ancho = ancho.ToString();
            barcode.alto = alto.ToString();
            barcode.rotate = rotate.ToString();


            string tipoCodigo = "";

           

            if (!IsValidGtin(GTIN) && TIPO != "qrcode" && TIPO != "gs1datamatrix")
            {
                barcode.errors = "Error: se proporcionó un dígito de control " + TIPO.ToUpper() + " incorrecto";
            }
            else
            {

         
            barcode.TIPOCOGNEX = tipoCodigo;
            barcode.TIPO = TIPO;

            if (formato == null)
            {                ///esta es la productiva
                barcode.archivo = GTIN + ".jpg";
                if (TIPO == "qrcode" || TIPO == "gs1datamatrix")
                {
                        //Codigo url con key privada, quitada para dicha subida.
                        GetImage(barcode.urlBarCode);
                        barcode.errors = errorBarCode;
                }
                else if(TIPO == "upca")
                    {
                        //Codigo url con key privada, quitada para dicha subida.
                        GetImage(barcode.urlBarCode);
                        barcode.errors = errorBarCode;
                    }
                else
                {
                       //Codigo url con key privada, quitada para dicha subida.
                        GetImage(barcode.urlBarCode);
                        barcode.errors = errorBarCode;
                    }
                 
            }
            else
            {

          
                ///Esta es la de test
                barcode.archivo ="barcode." + formato;
                if (TIPO == "qrcode" || TIPO ==  "gs1datamatrix")
                {
                        //Codigo url con key privada, quitada para dicha subida.
                }
                    else if (TIPO == "upca")
                    {
                        try
                        {
                               //Codigo url con key privada, quitada para dicha subida.
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                  
                    }
                    else
                    {
                        try
                        {
                           //Codigo url con key privada, quitada para dicha subida.
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }
               

               

            }
            }
    

           
            return barcode;
        }

        public async Task<ActionResult> GetBarcode(string GTIN, string TIPO, int ancho, int alto,int rotate, string formato)
        {
            CodigoDeBarras x = new CodigoDeBarras();
            x = GenerarCodigo(GTIN, TIPO, ancho, alto,rotate, formato);
          

            string base64 = await GetImage(x.urlBarCode);
            if(base64 == "-1")
            {
                return base.File("../Images/gallery.png", "image/png");
            }

            var userId = accountService.GetClaims("id");
            codeDataApi.SaveLogDownloadSymbols(userId, TIPO);
            return base.File(System.Convert.FromBase64String(base64), GTIN + ".jpg");

        }
      
        public async Task<ActionResult> DownloadBarCode(string GTIN, string TIPO, int ancho, int alto,int rotate, string formato)
        {
            
            CodigoDeBarras x = GenerarCodigo(GTIN, TIPO, ancho, alto, rotate, formato);

            string base64 = await GetImage(x.urlBarCode);

            byte[] imageBytes = Convert.FromBase64String(base64);
            string contentType = "Image/"+ formato;

            if (imageBytes == null)
            {
                return this.Content("No picture for this program.");
            }
            
 
            return File(imageBytes, contentType, "barcode." +formato);

        }

        public async Task<string> GetImage(string url)
        {
            var errors = new { errormsg = "" };
 
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);
                if(response.ContentType == "application/json")
                {
                var a = JsonConvert.DeserializeAnonymousType(response.Content, errors);
                if(a.errormsg.Contains("Too short"))
                {
                    errorBarCode = "El número es demasiado corto";
                }
                else if(a.errormsg.Contains("Too long"))
                {
                    errorBarCode = "El número es demasiado largo";
                }
                else if (a.errormsg.Contains("("))
                {
                    errorBarCode = "Tiene que comenzar con ()";
                }
                else
                {
                    errorBarCode = "Digito verificador invalido";
                }
               
                return "-1";
                }

            return Convert.ToBase64String(response.RawBytes);

        }
    }
}