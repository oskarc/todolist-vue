using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class ValuesController : ControllerBase
    //{
    //    // GET api/values
    //    [HttpGet]
    //    public ActionResult<IEnumerable<string>> Get()
    //    {
    //        var items = new List<int?>();
    //        var lists = new List<ListItem>();
    //        List<List> list = new List<List>();

    //        using (var db = new TodoListContext())
    //        {
    //            foreach (var item in db.ListItem)
    //            {
    //                items.Add(item.ListId);
    //                lists.Add(item);
    //            }
    //        }
    //        items = items.Distinct().ToList();
    //        if (items.Count > 0)
    //            return Ok(lists);
    //        else
    //            return NotFound("shit failed");
    //    }

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var items = new List<int?>();
            //var lists = new List<ListItem>();
            List<List> lists = new List<List>();

            using (var db = new TodoListContext())
            {
                foreach (var item in db.List)
                {
                    items.Add(item.ListId);
                    lists.Add(item);
                }
                foreach (var list in lists)
                {
                    foreach (var item in db.ListItem)
                    {
                        if (list.ListId == item.ListId && list.UserId == item.UserId)
                            list.ListItem.Add(item);
                    }
                }
            }
            items = items.Distinct().ToList();
            if (items.Count > 0)
                return Ok(lists);
            else if (items.Count == 0)
                return NoContent();
            else
                return BadRequest();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            List<string> listitems = new List<string>();

            using (var db = new TodoListContext())
            {
                foreach (var item in db.ListItem)
                {
                    if (item.ListId == id)
                        listitems.Add(item.ListItemText);
                }
            }
            if (listitems.Count > 0)
                return Ok(listitems);
            else
                return NoContent();
        }

        [Route("new/{id}/{userid}")]
        [HttpGet]
        public ActionResult<string> newGet(int id, int userid)
        {
            List<string> listitems = new List<string>();

            using (var db = new TodoListContext())
            {
                foreach (var item in db.ListItem)
                {
                    if (item.ListId == id && item.UserId == userid)
                        listitems.Add(item.ListItemText);
                }
            }
            if (listitems.Count > 0)
                return Ok(listitems);
            else
                return NoContent();
        }

        [Route("newPost")]
        [HttpPost]
        public ActionResult<string> newPost([FromBody] getListsByUser input)
        {
            List<string> listitems = new List<string>();

            using (var db = new TodoListContext())
            {
                foreach (var item in db.ListItem)
                {
                    if (item.ListId == input.ListId && item.UserId == input.UserId)
                        listitems.Add(item.ListItemText);
                }
            }
            if (listitems.Count > 0)
                return Ok(listitems);
            else
                return NoContent();
        }

        //// POST api/values
        //[HttpPost]
        //public IActionResult Post([FromBody] List<JSONListModel> model)
        //{
        //    if (!ModelState.IsValid)
        //        return StatusCode(404);
        //    else
        //    {
        //        using (var db = new TodoListContext())
        //        {

        //            foreach (var item in model)
        //            {
        //                List<int> NumberofLists = new List<int>();
        //                List<int> NumberofUsers = new List<int>();

        //                bool userExist = false;

        //                int actual_userId = 0;
        //                int actual_listId = 0;

        //                foreach (var y in db.List)
        //                {
        //                    if (item.UserId == y.UserId)
        //                    {
        //                        userExist = true;
        //                        NumberofLists.Add(y.ListId);
        //                    }
        //                    NumberofUsers.Add(y.UserId);
        //                }
        //                if (userExist == true && NumberofLists.Count() > 0)
        //                {
        //                    actual_userId = item.UserId;
        //                    actual_listId = NumberofLists.Max() + 1;
        //                }
        //                if (userExist == true && NumberofLists.Count() == 0)
        //                {
        //                    actual_userId = item.UserId;
        //                    actual_listId = 1;
        //                }
        //                if (userExist == false)
        //                {
        //                    if (NumberofUsers.Count() == 0)
        //                        actual_userId = 1;
        //                    else
        //                        actual_userId = NumberofUsers.Max() + 1;

        //                    actual_listId = 1;
        //                }
        //                if (actual_userId == 0 || actual_listId == 0)
        //                {
        //                    return BadRequest("shit happened");
        //                }
        //                else
        //                {
        //                    db.List.Add(
        //                           new List
        //                           {
        //                               ListId = actual_listId,
        //                               UserId = actual_userId
        //                           });

        //                    foreach (var z in item.List)
        //                    {
        //                        db.ListItem.Add(
        //                                 new ListItem
        //                                 {
        //                                     ListItemText = z.ListItemText,
        //                                     Completed = z.Completed,
        //                                     ListId = actual_listId,
        //                                     UserId = actual_userId
        //                                 });
        //                    }
        //                    db.SaveChanges();
        //                }
        //            }
        //        }
        //        return Ok("returned here");
        //    }

        //}

        // this is for adding 1 new list
        [HttpPost]
        public IActionResult Post([FromBody] JSONListModel model)
        {
            DateTime now = DateTime.Now;
            now = DateTime.ParseExact(now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null);
            if (!ModelState.IsValid)
                return StatusCode(404);
            else
            {
                using (var db = new TodoListContext())
                {
                    List<int> NumberofLists = new List<int>();
                    List<int> NumberofUsers = new List<int>();

                    bool userExist = false;

                    int actual_userId = 0;
                    int actual_listId = 0;

                    foreach (var y in db.List)
                    {
                        if (model.UserId == y.UserId)
                        {
                            userExist = true;
                            NumberofLists.Add(y.ListId);
                        }
                        NumberofUsers.Add(y.UserId);
                    }
                    if (userExist == true && NumberofLists.Count() > 0)
                    {
                        actual_userId = model.UserId;
                        actual_listId = NumberofLists.Max() + 1;
                    }
                    if (userExist == true && NumberofLists.Count() == 0)
                    {
                        actual_userId = model.UserId;
                        actual_listId = 1;
                    }
                    if (userExist == false)
                    {
                        if (NumberofUsers.Count() == 0)
                            actual_userId = 1;
                        else
                            actual_userId = NumberofUsers.Max() + 1;

                        actual_listId = 1;
                    }
                    if (actual_userId == 0 || actual_listId == 0)
                    {
                        return BadRequest("shit happened");
                    }
                    else
                    {
                        db.List.Add(
                               new List
                               {
                                   ListId = actual_listId,
                                   UserId = actual_userId,
                                   Title = model.Title
                               });

                        foreach (var z in model.ListItem)
                        {
                            db.ListItem.Add(
                                     new ListItem
                                     {
                                         ListItemText = z.ListItemText,
                                         Completed = z.Completed,
                                         ListId = actual_listId,
                                         UserId = actual_userId,
                                         Created = now
                                     });
                        }
                        db.SaveChanges();
                    }
                }
            }
            return Ok("returned here");

        }
        // this is for saving changes to existing lists
        [Route("Save")]
        [HttpPost]
        public IActionResult SaveChanges([FromBody] List<List> modelList)
        {
            if (!ModelState.IsValid)
                return StatusCode(404);
            else
            {
                using (var db = new TodoListContext())
                {

                    foreach (var list in modelList)
                    {
                        foreach (var listitem in list.ListItem)
                        {
                            foreach (var item in db.ListItem)
                            {
                                if (item.Id == listitem.Id)
                                    item.Completed = listitem.Completed;
                            }
                        }
                    }
                    int count = db.ListItem.Count();
                    db.SaveChanges();
                }
                return Ok();
            }
        }

        [Route("SaveItem")]
        [HttpPost]
        public IActionResult SaveItem([FromBody] ListItem listitem)
        {
            if (!ModelState.IsValid)
                return StatusCode(404);
            else
            {
                using (var db = new TodoListContext())
                {
                            foreach (var item in db.ListItem)
                            {
                                if (item.Id == listitem.Id)
                                    item.Completed = listitem.Completed;
                            }
                    db.SaveChanges();
                }
                return Ok();
            }
        }

        [Route("AddItem")]
        [HttpPost]
        public IActionResult AddItem([FromBody] ListItem listitem)
        {
            DateTime now = DateTime.Now;
            now = DateTime.ParseExact(now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null);

            if (!ModelState.IsValid)
                return StatusCode(404);
            else
            {
                using (var db = new TodoListContext())
                {
                    db.ListItem.Add(new ListItem
                    {
                        Completed = false,
                        ListId = listitem.ListId,
                        UserId = listitem.UserId,
                        ListItemText = listitem.ListItemText,
                        Created = now
                    });
                    db.SaveChanges();
                }
                return Ok();
            }
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult<IEnumerable<string>> Delete(int id)
        {
            using (var db = new TodoListContext())
            {
                foreach (var list in db.List)
                {
                    if (list.Id == id)
                    {
                        int userid = list.UserId;
                        db.List.Remove(list);

                        foreach (var item in db.ListItem)
                        {
                            if (item.ListId == list.ListId && item.UserId == userid)
                                db.ListItem.Remove(item);
                        }
                    }
                }
                db.SaveChanges();
            }
            return Get();
        }

        [Route("DeleteItem/{id}")]
        [HttpDelete]
        public ActionResult<IEnumerable<string>> DeleteItem(int id)
        {
            using (var db = new TodoListContext())
            {
                foreach (var item in db.ListItem)
                {
                    if (item.Id == id)
                        db.ListItem.Remove(item);
                }
                db.SaveChanges();
            }

            return Get();
        }



    }
}
