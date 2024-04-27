using eSya.DocumentControl.DL.Entities;
using eSya.DocumentControl.DO;
using eSya.DocumentControl.IF;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
        

        #region Define Business - Document Link -New
        public async Task<List<DO_FormBusinessLink>> GetMenuFormslinkwithLocation(int businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var ds = await db.GtEcbsmns.Where(x => x.BusinessKey == businesskey && x.ActiveStatus == true)
                        .Join(db.GtEcmnfls.Where(x => x.ActiveStatus),
                        f => f.MenuKey,
                        p => p.MenuKey,
                        (f, p) => new { f, p })
                         .Join(db.GtEcfmfds.Where(x => x.ActiveStatus),
                        fd => fd.p.FormId,
                        fp => fp.FormId,
                        (fd, fp) => new { fd, fp })
                        .Join(db.GtEcfmpas.Where(x => x.ParameterId == 2),
                        fpp => fpp.fp.FormId,
                        pm => pm.FormId,
                        (fpp, pm) => new { fpp, pm })
                        
                        .Select( r=> new DO_FormBusinessLink
                        {
                            BusinessKey = businesskey,
                            FormId = r.fpp.fp.FormId,
                            FormCode = r.fpp.fp.FormCode == null ? "" : r.fpp.fp.FormCode,
                            FormName = r.fpp.fp.FormName,
                            ActiveStatus = r.fpp.fp.ActiveStatus,

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

        public async Task<List<DO_DocumentControlStandard>> GetDocumentControlStandard(int formId, int businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var result = db.GtDccnsts.Where(x => x.ActiveStatus)
                        .Join(db.GtDncnms.Where(x=>x.ActiveStatus),
                         ds => ds.DocumentId,
                         dm => dm.DocumentId,
                        (ds, dm) => new { ds, dm })
                       .GroupJoin(db.GtDncnbcs.Where(w =>w.BusinessKey == businesskey),
                        df =>df.ds.ComboId,
                        d => d.ComboId,
                        (df, d) => new { df, d })
                       .SelectMany(z => z.d.DefaultIfEmpty(),
                       (a, b) => new DO_DocumentControlStandard
                      {
                          FormId = formId,
                          BusinessKey = businesskey,
                          DocumentId = a.df.ds.DocumentId,
                          ComboId = a.df.ds.ComboId,
                          GeneLogic = a.df.ds.GeneLogic,
                          CalendarType = a.df.ds.CalendarType,
                          IsTransationMode = a.df.ds.IsTransationMode,
                          IsStoreCode = a.df.ds.IsStoreCode,
                          IsPaymentMode = a.df.ds.IsPaymentMode,
                          SchemaId = a.df.ds.SchemaId,
                          DocumentDesc = a.df.dm.DocumentDesc,
                          ShortDesc = a.df.dm.ShortDesc,
                          DocumentType = a.df.dm.DocumentType,
                          UsageStatus = b == null ? false : b.ActiveStatus,
                          ActiveStatus = b == null ? false : b.ActiveStatus,
                       
                    }).ToListAsync();

                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DO_ReturnParameter> InsertOrUpdateBusinesswiseDocumentControlLink(List<DO_BusinessDocument_Link> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {

                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        foreach (var d in obj)
                        {
                            var calkey = db.GtEcblcls.Where(x => x.BusinessKey == d.BusinessKey && x.ActiveStatus && x.CalenderKey.ToUpper().Replace(" ", "").StartsWith(d.CalendarType.ToUpper().Replace(" ", ""))).FirstOrDefault();
                            if (calkey == null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0115", Message = string.Format(_localizer[name: "W0115"]) };

                            }
                            else
                            {
                                d.CalendarKey = calkey.CalenderKey;
                            }
                        }

                        foreach (var objadd in obj)
                        {
                            var docctrl = db.GtDncnbcs.Where(x => x.BusinessKey == objadd.BusinessKey && x.CalendarKey.ToUpper().Replace(" ", "") == objadd.CalendarKey.ToUpper().Replace(" ", "") && x.ComboId == objadd.ComboId).FirstOrDefault();

                            if (docctrl != null)
                            {
                                docctrl.FormId = objadd.FormId;
                                docctrl.DocumentId = objadd.DocumentId;
                                docctrl.SchemaId = objadd.SchemaId;
                                docctrl.UsageStatus = objadd.ActiveStatus;
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
                                    UsageStatus = objadd.ActiveStatus,
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
