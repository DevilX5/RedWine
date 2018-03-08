using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedWine.Model;

namespace RedWine.Controllers
{
    [Produces("application/json")]
    [Route("api/Test")]
    public class TestController : Controller
    {
        // GET: api/Test
        [HttpGet]
        public async Task<IEnumerable<Test1>> Get()
        {
            return await DBHelper.GetAll<Test1>();
        }

        // GET: api/Test/5
        [HttpGet("{id}")]
        public async Task<Test1> Get(int id)
        {
            return await DBHelper.AsyncGet<Test1>(id);
        }
        
        // POST: api/Test
        [HttpPost]
        public async Task<EasyuiPaged<Test1>> Post(TestEpq o)
        {
            var sql = "select * from Test1";
            var pagesql =sql+ $" limit {o.start},{o.rows}";
            var countsql = $"select count(*) from ({sql}) t";
            var data = await DBHelper.AsyncQuery<Test1>(pagesql);
            var total = await DBHelper.AsyncExecuteScalar(countsql);
            return new EasyuiPaged<Test1>(total.ToInt(), data);
        }
        
        // PUT: api/Test/5
        [HttpPut]
        public async Task<bool> Put(Test1 o)
        {
            if (o.Id.HasValue)
            {
                return await DBHelper.AsyncInsert(o);
            }
            else
            {
                return await DBHelper.AsyncUpdate(o);
            }
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<bool> Delete(Test1 o)
        {
            return await DBHelper.AsyncDelete(o);
        }
        
    }
}
