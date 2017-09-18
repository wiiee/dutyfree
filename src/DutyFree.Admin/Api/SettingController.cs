namespace DutyFree.Admin.Api
{
    using WebCore.Base;
    using Entity.Standard;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Platform.Util;
    using Service.Model;
    using Service.Result;
    using Service.Standard;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Route("api/[controller]")]
    public class SettingController : BaseController
    {
        private static ILogger _logger = LoggerUtil.CreateLogger<SettingController>();

        private PropertyService _propertyService;
        private CategoryService _categoryService;
        private UnitService _unitService;
        private BrandService _brandService;
        private AirportService _airportService;
        private RegionService _regionService;

        public SettingController(PropertyService propertyService, CategoryService categoryService, UnitService unitService, BrandService brandService,
            AirportService airportService, RegionService regionService)
        {
            this._propertyService = propertyService;
            this._categoryService = categoryService;
            this._unitService = unitService;
            this._brandService = brandService;
            this._airportService = airportService;
            this._regionService = regionService;
        }

        [Route("GetProperties")]
        [HttpPost]
        public List<Property> GetProperties()
        {
            try
            {
                return this._propertyService.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [Route("PostProperties")]
        [HttpPost]
        public ServiceResult PostProperties([FromBody]List<Property> entities)
        {
            var result = new ServiceResult();

            try
            {
                var ids = _propertyService.GetIds();

                var deleteIds = ids.Except(entities.Select(o => o.Id)).ToList();
                var addEntities = entities.Where(o => !ids.Contains(o.Id)).ToList();
                var updateEntities = entities.Where(o => ids.Contains(o.Id)).ToList();

                _propertyService.Delete(deleteIds);
                _propertyService.Create(addEntities);
                _propertyService.Update(updateEntities);

                result.IsSuccessful = true;
                result.Msg = "Update properties successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.IsSuccessful = false;
                result.Msg = ex.Message;
            }

            return result;
        }

        [Route("GetCategoryChain")]
        [HttpPost]
        public List<List<string>> GetCategoryChain(string categoryId)
        {
            try
            {
                return this._categoryService.GetCategoryChain(categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [Route("GetCategoriesByChild")]
        [HttpPost]
        public List<string> GetCategoriesByChild(string categoryId)
        {
            try
            {
                return this._categoryService.GetCategoriesByChild(categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [Route("GetCategories")]
        [HttpPost]
        public List<Category> GetCategories()
        {
            try
            {
                return this._categoryService.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [Route("GetCategoryLinkedList")]
        [HttpPost]
        public List<LinkedList> GetCategoryLinkedList()
        {
            try
            {
                return this._categoryService.GetCategories();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [Route("PostCategories")]
        [HttpPost]
        public ServiceResult PostCategories([FromBody]List<Category> entities)
        {
            var result = new ServiceResult();

            try
            {
                var ids = _categoryService.GetIds();

                var deleteIds = ids.Except(entities.Select(o => o.Id)).ToList();
                var addEntities = entities.Where(o => !ids.Contains(o.Id)).ToList();
                var updateEntities = entities.Where(o => ids.Contains(o.Id)).ToList();

                _categoryService.Delete(deleteIds);
                _categoryService.Create(addEntities);
                _categoryService.Update(updateEntities);

                result.IsSuccessful = true;
                result.Msg = "Update categories successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.IsSuccessful = false;
                result.Msg = ex.Message;
            }

            return result;
        }

        [Route("GetUnits")]
        [HttpPost]
        public List<Unit> GetUnits()
        {
            try
            {
                return this._unitService.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [Route("PostUnits")]
        [HttpPost]
        public ServiceResult PostUnits([FromBody]List<Unit> entities)
        {
            var result = new ServiceResult();

            try
            {
                var ids = _unitService.GetIds();

                var deleteIds = ids.Except(entities.Select(o => o.Id)).ToList();
                var addEntities = entities.Where(o => !ids.Contains(o.Id)).ToList();
                var updateEntities = entities.Where(o => ids.Contains(o.Id)).ToList();

                _unitService.Delete(deleteIds);
                _unitService.Create(addEntities);
                _unitService.Update(updateEntities);

                result.IsSuccessful = true;
                result.Msg = "Update units successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.IsSuccessful = false;
                result.Msg = ex.Message;
            }

            return result;
        }

        [Route("GetBrands")]
        [HttpPost]
        public List<Brand> GetBrands()
        {
            try
            {
                return this._brandService.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [Route("PostBrands")]
        [HttpPost]
        public ServiceResult PostBrands([FromBody]List<Brand> entities)
        {
            var result = new ServiceResult();

            try
            {
                var ids = _brandService.GetIds();

                var deleteIds = ids.Except(entities.Select(o => o.Id)).ToList();
                var addEntities = entities.Where(o => !ids.Contains(o.Id)).ToList();
                var updateEntities = entities.Where(o => ids.Contains(o.Id)).ToList();

                _brandService.Delete(deleteIds);
                _brandService.Create(addEntities);
                _brandService.Update(updateEntities);

                result.IsSuccessful = true;
                result.Msg = "Update brands successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.IsSuccessful = false;
                result.Msg = ex.Message;
            }

            return result;
        }


        [Route("GetAirports")]
        [HttpPost]
        public List<Airport> GetAirports()
        {
            try
            {
                return this._airportService.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [Route("PostAirports")]
        [HttpPost]
        public ServiceResult PostAirports([FromBody]List<Airport> entities)
        {
            var result = new ServiceResult();

            try
            {
                var ids = _airportService.GetIds();

                var deleteIds = ids.Except(entities.Select(o => o.Id)).ToList();
                var addEntities = entities.Where(o => !ids.Contains(o.Id)).ToList();
                var updateEntities = entities.Where(o => ids.Contains(o.Id)).ToList();

                _airportService.Delete(deleteIds);
                _airportService.Create(addEntities);
                _airportService.Update(updateEntities);

                result.IsSuccessful = true;
                result.Msg = "Update airports successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.IsSuccessful = false;
                result.Msg = ex.Message;
            }

            return result;
        }

        [Route("GetRegions")]
        [HttpPost]
        public List<Region> GetRegions()
        {
            try
            {
                return this._regionService.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [Route("PostRegions")]
        [HttpPost]
        public ServiceResult PostRegions([FromBody]List<Region> entities)
        {
            var result = new ServiceResult();

            try
            {
                var ids = _regionService.GetIds();

                var deleteIds = ids.Except(entities.Select(o => o.Id)).ToList();
                var addEntities = entities.Where(o => !ids.Contains(o.Id)).ToList();
                var updateEntities = entities.Where(o => ids.Contains(o.Id)).ToList();

                _regionService.Delete(deleteIds);
                _regionService.Create(addEntities);
                _regionService.Update(updateEntities);

                result.IsSuccessful = true;
                result.Msg = "Update regions successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.IsSuccessful = false;
                result.Msg = ex.Message;
            }

            return result;
        }
    }
}
