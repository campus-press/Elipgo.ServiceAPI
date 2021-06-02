using AutoMapper;
using Examen.Elipgo.DAL.Contexts;
using Examen.Elipgo.DAL.Models;
using Examen.Elipgo.DAO.Interfaces;
using Examen.Elipgo.DAO.Models;
using Examen.Elipgo.DAO.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Examen.Elipgo.DAO.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ArticleRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StatusResponse<IEnumerable<ArticleDAO>>> Get()
        {
            var data = await _context.Articles.Include(x => x.Store).ToListAsync();
            var listResponse = data.Select(x => new ArticleDAO()
            {
                Price = x.Price,
                StoreId = x.StoreId,
                Code = x.Code,
                Description = x.Description,
                Store = new StoreDAO(){ Name = x.Store.Name, Address = x.Store.Address, Id = x.Store.Id },
                Name = x.Name,
                Quantity = x.Quantity,
                Id = x.Id,
                TotalInShelf = x.TotalInShelf,
                TotalInVault = x.TotalInVault
            }).ToList();

            return new StatusResponse<IEnumerable<ArticleDAO>>()
            {
                Success = true,
                Message = "Success",
                StatusCode = HttpStatusCode.OK,
                Value = listResponse
            };
        }

        public async Task<StatusResponse<ArticleDAO>> Get(int id)
        {
            var data = await _context.Articles.Include(x => x.Store).FirstOrDefaultAsync(x => x.Id == id);

            if (data != null)
            {
                var response = new ArticleDAO()
                {
                    Price = data.Price,
                    StoreId = data.StoreId,
                    Code = data.Code,
                    Description = data.Description,
                    Store = new StoreDAO() { Name = data.Store.Name, Address = data.Store.Address, Id = data.Store.Id },
                    Name = data.Name,
                    Quantity = data.Quantity,
                    Id = data.Id,
                    TotalInShelf = data.TotalInShelf,
                    TotalInVault = data.TotalInVault
                };
                return new StatusResponse<ArticleDAO>()
                {
                    Success = true,
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Value = response
                };
            }
            else
            {
                return new StatusResponse<ArticleDAO>()
                {
                    Success = false,
                    Message = "Record not Found",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<StatusResponse<List<ArticleDAO>>> Get(List<KeyValuePair<string, string>> paramList)
        {
            System.Linq.IQueryable<Article> query = _context.Articles.Include(s => s.Store);
            try
            {
                query = paramList.Aggregate(query, (current, pair) => pair.Key switch
                {
                    "Name" => current.Where(x => x.Name.Contains(pair.Value)),
                    "Id" => current.Where(x => x.Id == Convert.ToInt32(pair.Value)),
                    "Store" => current.Where(x => x.Store.Name.Equals(pair.Value)),
                    _ => current
                });
                var list =  await query.ToListAsync();
                return new StatusResponse<List<ArticleDAO>>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Filtro Aplicado correctamente",
                    Success = true,
                    Value = (from a in list
                        select new ArticleDAO()
                        {
                            Id = a.Id,
                            Store = new StoreDAO(){Id = a.Store.Id, Address = a.Store.Address, Name = a.Store.Name},
                            Price = a.Price,
                            StoreId = a.StoreId,
                            Code = a.Code,
                            Name = a.Name,
                            Description = a.Description,
                            Quantity = a.Quantity,
                            TotalInShelf = a.TotalInShelf,
                            TotalInVault = a.TotalInVault
                        }).ToList()
            };
            }
            catch (Exception e)
            {
                return new StatusResponse<List<ArticleDAO>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = e.Message,
                    Success = false,
                };
            }
        }

        public async Task<StatusResponse<IEnumerable<ArticleDAO>>> GetArticlesByStore(int storeId)
        {
            var data = await _context.Articles
                .Include(x => x.Store)
                .Where(x => x.StoreId == storeId)
                .ToListAsync();
            if (data.Count > 0)
            {
                var listResponse = data.Select(x => new ArticleDAO()
                {
                    Price = x.Price,
                    StoreId = x.StoreId,
                    Code = x.Code,
                    Description = x.Description,
                    Store = new StoreDAO() { Name = x.Store.Name, Address = x.Store.Address, Id = x.Store.Id },
                    Name = x.Name,
                    Quantity = x.Quantity,
                    Id = x.Id,
                    TotalInShelf = x.TotalInShelf,
                    TotalInVault = x.TotalInVault
                }).ToList();

                return new StatusResponse<IEnumerable<ArticleDAO>>()
                {
                    Success = true,
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Value = listResponse
                };
            }
            else
            {
                return new StatusResponse<IEnumerable<ArticleDAO>>()
                {
                    Success = false,
                    Message = "Record not Found",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            
        }

        public async Task<StatusResponse> Post(ArticleDAO articleDao)
        {
            try
            {
                var map = _mapper.Map<Article>(articleDao);
                await _context.Articles.AddAsync(map);
                await _context.SaveChangesAsync();
                return new StatusResponse()
                {
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Artículo Agregado Satisfactoriamente"
                };
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    return new StatusResponse()
                    {
                        Success = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "No se puede agregar un producto nuevo con códigos iguales, favor de verificar"
                    };
                }
                return new StatusResponse()
                {
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }
            
        }

        public async Task<StatusResponse> Put(ArticleDAO articleDao)
        {
            try
            {
                var map = _mapper.Map<Article>(articleDao);
                _context.Articles.Update(map);
                await _context.SaveChangesAsync();
                return new StatusResponse()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Articulo Actualizada Correctamente",
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
                var data = await _context.Articles.FindAsync(id);
                _context.Articles.Remove(data);
                await _context.SaveChangesAsync();
                return new StatusResponse()
                {
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Articulo eliminado correctamente"
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
