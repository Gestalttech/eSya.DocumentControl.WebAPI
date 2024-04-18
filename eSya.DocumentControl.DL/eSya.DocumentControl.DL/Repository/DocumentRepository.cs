using eSya.DocumentControl.DL.Entities;
using eSya.DocumentControl.DO;
using eSya.DocumentControl.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.DocumentControl.DL.Repository
{
    public class DocumentRepository: IDocumentRepository
    {
        private readonly IStringLocalizer<DocumentRepository> _localizer;
        public DocumentRepository(IStringLocalizer<DocumentRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Define Business - Document Link
        public async Task<List<DO_BusinessCalendarLink>> GetBusinesslinkedCalendarkeys()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {

                    var result = await db.GtEcblcls.Where(x => x.ActiveStatus)
                        .Select(x => new DO_BusinessCalendarLink
                        {
                            CalenderKey = x.CalenderKey
                        }).ToListAsync();
                    var Distinctcals = result.GroupBy(x => x.CalenderKey).Select(y => y.First());
                    return Distinctcals.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_BusinessLocation>> GetBusinessLocationbyCalendarkeys(string calendarkey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {

                    var locs =await db.GtEcblcls.Where(x => x.CalenderKey == calendarkey && x.ActiveStatus)
                         .Join(db.GtEcbslns,
                          x => x.BusinessKey,
                          y => y.BusinessKey,
                         (x, y) => new DO_BusinessLocation
                         {
                             BusinessKey = x.BusinessKey,
                             LocationDescription = y.BusinessName + "-" + y.LocationDescription

                         }).ToListAsync();

                    var Distinctcals = locs.GroupBy(x => x.BusinessKey).Select(y => y.First());
                    return Distinctcals.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_BusinessDocument_Link>> GetDocumentFormlinkwithLocation(string calendarkey,int businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var ds = await db.GtEcfmfds.Where(x => x.ActiveStatus == true)
                        .Join(db.GtEcfmpas.Where(x => x.ParameterId == 2),
                        f => f.FormId,
                        p => p.FormId,
                        (f, p) => new { f, p })
                         .Join(db.GtEcfmpas.Where(x => x.ParameterId == 1),
                        fd => fd.f.FormId,
                        fp => fp.FormId,
                        (fd, fp) => new { fd, fp })
                   .GroupJoin(db.GtDncnbcs.Where(w => w.CalendarKey == calendarkey && w.BusinessKey==businesskey),
                     df =>df.fd.f.FormId,
                     l => l.FormId,
                    (form, doc) => new { form, doc })
                   .SelectMany(z => z.doc.DefaultIfEmpty(),
                    (a, b) => new DO_BusinessDocument_Link
                    {
                        FormId = a.form.fd.f.FormId,
                        FormName = a.form.fd.f.FormName,
                        BusinessKey = b == null ? businesskey : b.BusinessKey,
                        CalendarKey = b == null ? calendarkey : b.CalendarKey,
                        ComboId = b == null ? 0 : b.ComboId,
                        DocumentId = b == null ? 0 : b.DocumentId,
                        SchemaId=b == null ? "": b.SchemaId,
                        UsageStatus = b == null ? false : b.UsageStatus,
                        FreezeStatus = b == null ? false : b.FreezeStatus,
                        ActiveStatus = b == null ? false : b.ActiveStatus,
                        
                    }).ToListAsync();

                    var Distinctform = ds.GroupBy(x => x.FormId).Select(y => y.First());
                    return Distinctform.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_DocumentControlMaster>> GetDocumentControlMaster(int formId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtDccnsts.Select(
                        s => new DO_DocumentControlMaster
                        {
                            FormId = formId.ToString(),
                            DocumentId = s.DocumentId,
                            GeneLogic = s.GeneLogic,
                            CalendarType = s.CalendarType,
                            IsTransationMode = s.IsTransationMode,
                            IsStoreCode = s.IsStoreCode,
                            IsPaymentMode = s.IsPaymentMode,
                            SchemaId = s.SchemaId,
                            ComboId = s.ComboId,
                            DocumentDesc = s.DocumentDesc,
                            ShortDesc = s.ShortDesc,
                            DocumentType = s.DocumentType,
                            UsageStatus = s.UsageStatus,
                            //ActiveStatus = s.ActiveStatus
                            ActiveStatus=false
                        }).ToListAsync();

                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateBusinesswiseDocumentControl(List<DO_BusinessDocument_Link> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {

                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach(var d in obj.Where(x=>x.ActiveStatus==false))
                        {
                            var lstdoc =await db.GtDncnbcs.Where(x => x.BusinessKey == d.BusinessKey && x.CalendarKey.ToUpper().Replace(" ", "") == d.CalendarKey.ToUpper().Replace(" ", "") && x.ComboId == d.ComboId
                            && x.FormId == d.FormId && x.DocumentId == d.DocumentId && x.SchemaId.ToUpper().Replace(" ", "") == d.SchemaId.ToUpper().Replace(" ", "")).ToListAsync();
                            foreach(var del in lstdoc)
                            {
                                db.GtDncnbcs.Remove(del);
                                await db.SaveChangesAsync();
                            }
                        }

                        foreach (var objadd in obj.Where(x => x.ActiveStatus == true))
                        {
                            var docctrl = db.GtDncnbcs.Where(x => x.BusinessKey == objadd.BusinessKey && x.CalendarKey.ToUpper().Replace(" ", "") == objadd.CalendarKey.ToUpper().Replace(" ", "") && x.ComboId == objadd.ComboId).FirstOrDefault();

                            if (docctrl != null)
                            {
                                docctrl.FormId = objadd.FormId;
                                docctrl.DocumentId = objadd.DocumentId;
                                docctrl.SchemaId = objadd.SchemaId;
                                //docctrl.UsageStatus = false;
                                docctrl.FreezeStatus = objadd.FreezeStatus;
                                docctrl.ActiveStatus = objadd.ActiveStatus;
                                docctrl.ModifiedBy = objadd.UserID;
                                docctrl.ModifiedOn = System.DateTime.Now;
                                docctrl.ModifiedTerminal = objadd.TerminalID;
                                await db.SaveChangesAsync();
                             
                            }
                            else
                            {
                                GtDncnbc doc = new GtDncnbc()
                                {
                                    BusinessKey = objadd.BusinessKey,
                                    CalendarKey = objadd.CalendarKey,
                                    ComboId = objadd.ComboId,
                                    FormId = objadd.FormId,
                                    DocumentId = objadd.DocumentId,
                                    SchemaId = objadd.SchemaId,
                                    UsageStatus = false,
                                    FreezeStatus = objadd.FreezeStatus,
                                    ActiveStatus = objadd.ActiveStatus,
                                    CreatedBy = objadd.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = objadd.TerminalID
                                };
                                db.GtDncnbcs.Add(doc);
                                await db.SaveChangesAsync();
                            }
                        }
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

                    }

                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }

            }
        }
        #endregion
    }
}
