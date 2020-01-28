using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiJwt.Entities;
using WebApiJwt.Models;

namespace WebApiJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventController : ControllerBase
    {

        ApplicationDbContext _context;
        private readonly IHostingEnvironment hostingEnvironment;


        public EventController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
           
        }
        [HttpGet("GetClass")]
        public ActionResult gatClasses()
        {

            var d = _context.classes.ToList();
            return new JsonResult(d);
        }
        [HttpPost("SaveEvent")]
        public ActionResult SaveEvent([FromForm] EventDto eventDto)
        {
            if (eventDto.id == 0)
            {
                DateTime dateTime = DateTime.Parse(eventDto.startDateTime);

                DateTime enddate = DateTime.Parse(eventDto.endDateTime);
                Event @event = new Event();
                @event.title = eventDto.title;
                @event.description = eventDto.description;
                @event.teacher_id = eventDto.teacher_id;
                @event.endDateTime = enddate;
                @event.startDateTime = dateTime;
                @event.location = eventDto.location;
                _context.events.Add(@event);
                _context.SaveChanges();
                foreach (var v in eventDto.clasid)
                {
                    ClassEvent classEvent = new ClassEvent()
                    {
                        clasid = v,
                        eventID = @event.id

                    }

                    ;
                    _context.classEvents.Add(classEvent);
                    _context.SaveChanges();
                }
                var fileName = "";
                var deletefile = Path.Combine(hostingEnvironment.WebRootPath);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");

                if (eventDto.uploadFile != null)
                {


                    fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(eventDto.uploadFile.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        eventDto.uploadFile.CopyToAsync(fileStream);

                    }


                    EventAttachment eventAttachment = new EventAttachment()
                    {
                        event_id = @event.id,
                        file_name = eventDto.uploadFile.FileName,
                        file_path = fileName,
                        file_type = Path.GetExtension(eventDto.uploadFile.FileName),


                    };
                    _context.eventAttachments.Add(eventAttachment);
                    _context.SaveChanges();

                }
            }
            else
            {
                var data = _context.events.FirstOrDefault(o => o.id == eventDto.id);
                DateTime dateTime = DateTime.Parse(eventDto.startDateTime);

                DateTime enddate = DateTime.Parse(eventDto.endDateTime);
                data.title = eventDto.title;
                data.description = eventDto.description;
                data.teacher_id = eventDto.teacher_id;
                data.endDateTime = enddate;
                data.startDateTime = dateTime;
                data.location = eventDto.location;

                var claesfromevent = _context.classEvents.Where(o => o.eventID == data.id).ToList();

                foreach (var item in claesfromevent)
                {
                    _context.classEvents.Remove(item);
                    _context.SaveChanges();
                }
                    foreach (var v in eventDto.clasid)
                {
                    ClassEvent classEvent = new ClassEvent()
                    {
                        clasid = v,
                        eventID =data.id

                    }

                    ;
                    _context.classEvents.Add(classEvent);
                    _context.SaveChanges();
                }
                

                if (eventDto.uploadFile != null)
                {

                    var enevtAtachment = _context.eventAttachments.FirstOrDefault(o => o.event_id == data.id);

                    var fileName = "";
                    var deletefile = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath);
                    if(enevtAtachment!=null)
                    {
                        if (eventDto.uploadFile.Length > 0)
                        {
                            if (System.IO.File.Exists((Path.Combine(deletefile, enevtAtachment.file_path))))
                            {

                                System.IO.File.Delete((Path.Combine(deletefile, enevtAtachment.file_path)));

                            }
                    }

                        fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(eventDto.uploadFile.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            eventDto.uploadFile.CopyToAsync(fileStream);

                        }


                        enevtAtachment.file_path = fileName;
                        enevtAtachment.file_name = eventDto.uploadFile.FileName;
                        enevtAtachment.file_type = Path.GetExtension(eventDto.uploadFile.FileName);
                        _context.Entry(enevtAtachment).State=Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();

                    }



                } }




            return new JsonResult("Data Saved");
        }

        [HttpGet("GetEvent")]
        public ActionResult GetEvent(int? eventId = 0,int? classId=0 ,bool? past=null)
        {         
            var v = from t in _context.events
                    where t.is_deleted!=true
                    join teacher in _context.Users on  t.teacher_id equals teacher.Id
                    select new
                    {
                        t.id,
                        t.location,
                        t.title,t.description,
                        startTime = string.Format("{0:hh:mm:ss tt}", t.startDateTime.ToShortTimeString()),
                        StartDate =DateTime.Parse( t.startDateTime.ToString("ddd, dd MMM yyy")),
                       endTime = string.Format("{0:hh:mm:ss tt}", t.endDateTime.ToShortTimeString()),
                        endtDate = DateTime.Parse(t.endDateTime.ToString("ddd, dd MMM yyy")),
                        SDate =t.startDateTime.ToString("ddd, dd MMM yyy"),
                        eDate = t.endDateTime.ToString("ddd, dd MMM yyy"),
                        teacherName=teacher.FullName,
                        teacher_id=t.teacher_id,
                        coverphoto =_context.eventAttachments.Where(o=>o.event_id==t.id).ToList()
                        ,
                        clasees = (from k in _context.classEvents 
                                where k.is_deleted!=true
                                 where k.eventID ==t.id
                                 join clases in _context.classes on k.clasid equals clases.id
                                 select new
                                 {
                                     clases.level,
                                     id=k.clasid
                                 }
                                ).ToList()
                    }
                    ;
            if (eventId != 0)
            {
              return new JsonResult(  v.FirstOrDefault(o => o.id == eventId));

            }/*
            else if (startdate != null && endDate != null)
            {
                v.Where(o => o.StartDate >= startdate.Value && o.endtDate <= endDate).ToList();
            }*/
            else if (past == false)
            {

               return new JsonResult( v.Where(o => o.StartDate >= DateTime.UtcNow).ToList());

            }
            else if (past == true)
            {
                return new JsonResult(v.Where(o => o.StartDate <= DateTime.UtcNow).ToList());
              

            }
            else if (classId != 0)
            {
                var ClassData = from t in _context.events
                                join teacher in _context.Users on t.teacher_id equals teacher.Id
                                join clas in _context.classEvents on t.id equals clas.eventID
                                join cl in _context.classes on clas.clasid equals cl.id
                                where clas.clasid == classId && clas.is_deleted!=true
                                select new
                                {
                                    t.id,
                                    cl.level,
                                    teacher.FullName,
                                    t.title,
                                    t.description,
                                    startTime = string.Format("{0:hh:mm:ss tt}", t.startDateTime.ToShortTimeString()),
                                    StartDate = DateTime.Parse(t.startDateTime.ToString("ddd, dd MMM yyy")),
                                    endTime = string.Format("{0:hh:mm:ss tt}", t.endDateTime.ToShortTimeString()),
                                    endtDate = DateTime.Parse(t.endDateTime.ToString("ddd, dd MMM yyy")),




                                };
              
                return new JsonResult(ClassData.Where(o => o.StartDate >= DateTime.UtcNow).ToList());

            }
            else
            {
                v.ToList();

            }

            return new JsonResult(v);
        }

        [HttpGet("GetEventByDate")]
        public ActionResult GetEventByDate( string startD , string endD )
        {

            DateTime start = DateTime.Parse(startD);

            DateTime end = DateTime.Parse(endD);

            var v = (from t in _context.events
                    where t.is_deleted != true
                    join teacher in _context.Users on t.teacher_id equals teacher.Id
                    select new
                    {
                        t.id,
                        t.location,
                        t.title,
                        t.description,
                        startTime = string.Format("{0:hh:mm:ss tt}", t.startDateTime.ToShortTimeString()),
                        StartDate = DateTime.Parse(t.startDateTime.ToString("ddd, dd MMM yyy")),
                        endTime = string.Format("{0:hh:mm:ss tt}", t.endDateTime.ToShortTimeString()),
                        endtDate = DateTime.Parse(t.endDateTime.ToString("ddd, dd MMM yyy")),
                        SDate = t.startDateTime.ToString("ddd, dd MMM yyy"),
                        eDate = t.endDateTime.ToString("ddd, dd MMM yyy"),
                        teacherName = teacher.FullName,
                        coverphoto = _context.eventAttachments.Where(o => o.event_id == t.id).ToList()
                        ,
                        clasees = (from k in _context.classEvents
                                   where k.is_deleted != true
                                   where k.eventID == t.id
                                   join clases in _context.classes on k.clasid equals clases.id
                                   select new
                                   {
                                       clases.level,
                                       id = k.clasid
                                   }
                                ).ToList()
                    }).Where(O=>O.StartDate>=start && O.endtDate<=end)
                    ;
            return new JsonResult(v);
        }


        [HttpPost("FileUploadedByTeacher")]
        public IActionResult FileUploadedByTeacher([FromForm]List<EventAtachmentUpload> upload)
        {
            foreach (var item in upload)
            {

                var fileName = "";
                var deletefile = Path.Combine(hostingEnvironment.WebRootPath);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");

                if (item.upload != null)
                {


                    fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(item.upload.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        item.upload.CopyToAsync(fileStream);

                    }


                    EventAttachment eventAttachment = new EventAttachment()
                    {
                        event_id = item.event_id,
                        file_name = item.upload.FileName,
                        file_path = fileName,
                         mtype="Teacher",
                        file_type = Path.GetExtension(item.upload.FileName),


                    };
                    _context.eventAttachments.Add(eventAttachment);
                    _context.SaveChanges();

                }
            }
                return new JsonResult("Uploaded");
        }
    }
    }