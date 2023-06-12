namespace NomnbreNoReal
{
    using Core.Models;
    using Core.Contracts.Services;
    using Services;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;

    [Authorize]
    public class GeneradorCodigoController : ApiController
    {
        private readonly IGeneradorCodigoService codigoService;

        public GeneradorCodigoController()
            : this(new GeneradorCodigoService())
        {
        }

        public GeneradorCodigoController(IGeneradorCodigoService _codigoService)
        {
            codigoService = _codigoService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/GetTypesBarCodes")]
        public async Task<IList<TipoCodigosDeBarra>> GetTypesBarCodes()
        {
            return await codigoService.GetTypesBarCode();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/SaveLogDownloadSymbols")]
        public async Task<int> SaveLogDownloadSymbology(string userId, string tipo)
        {
            return await codigoService.SaveLogDownloadSymbols(userId, tipo);
          
        }
    }
}