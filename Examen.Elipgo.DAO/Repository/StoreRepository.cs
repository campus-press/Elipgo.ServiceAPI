using AutoMapper;
using Examen.Elipgo.DAL.Contexts;
using Examen.Elipgo.DAL.Models;
using Examen.Elipgo.DAO.Interfaces;
using Examen.Elipgo.DAO.Models;
using Examen.Elipgo.DAO.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Examen.Elipgo.DAO.Repository
{
    public class StoreRepository : IStoreRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StoreRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StatusResponse<IEnumerable<StoreDAO>>> Get()
        {
            var list = _mapper.Map<IEnumerable<StoreDAO>>( await _context.Stores.ToListAsync());
            return new StatusResponse<IEnumerable<StoreDAO>>()
            {
                Success = true,
                Message = "Success",
                StatusCode = HttpStatusCode.OK,
                Value = list
            };
        }

        public async Task<StatusResponse<StoreDAO>> Get(int id)
        {
            var data = _mapper.Map<StoreDAO>(await _context.Stores.FirstOrDefaultAsync(x => x.Id == id));
            if (data != null)
            {
                return new StatusResponse<StoreDAO>()
                {
                    Success = true,
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Value = data
                };
            }
            else
            {
                return new StatusResponse<StoreDAO>()
                {
                    Success = false,
                    Message = "Record not Found",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            
        }

        public async Task<StatusResponse> Post(StoreDAO articleDao)
        {
            try
            {
                var map = _mapper.Map<Store>(articleDao);
                await _context.Stores.AddAsync(map);
                await _context.SaveChangesAsync();
                return new StatusResponse()
                {
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Sucursal Agregada Satisfactoriamente"
                };
            }
            catch (Exception e)
            {
                return new StatusResponse()
                {
                    Success = true,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = e.Message
                };
            }
            
        }

        public async Task<StatusResponse> Put(StoreDAO articleDao)
        {
            try
            {
                var map = _mapper.Map<Store>(articleDao);
                _context.Stores.Update(map);
                await _context.SaveChangesAsync();
                return new StatusResponse()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Sucursal Actualizada Correctamente",
                    Success = true
                };
            }
            catch (DbUpdateException e)
            {
                return new StatusResponse()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = e.Message,
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new StatusResponse()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = e.Message,
                    Success = true
                };
            }
        }

        public async Task<StatusResponse> Delete(int id)
        {
            try
            {
                var data = await _context.Stores.FindAsync(id);
                _context.Stores.Remove(data);
                await _context.SaveChangesAsync();
                return new StatusResponse()
                {
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Sucursal eliminada correctamente"
                };
            }
            catch (Exception e)
            {
                return new StatusResponse()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = e.Message,
                    Success = false
                };
            }
            
        }
    }
}
