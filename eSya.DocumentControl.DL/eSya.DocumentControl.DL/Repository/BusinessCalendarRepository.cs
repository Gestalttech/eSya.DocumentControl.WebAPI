﻿using eSya.DocumentControl.DL.Entities;
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
    public class BusinessCalendarRepository : IBusinessCalendarRepository
    {
        private readonly IStringLocalizer<BusinessCalendarRepository> _localizer;
        public BusinessCalendarRepository(IStringLocalizer<BusinessCalendarRepository> localizer)
        {
            _localizer = localizer;
        }

        #region Calendar Business Link
        public async Task<List<DO_BusinessCalendarLink>> GetCalendarHeaders(int businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var result = await db.GtEcclcos

                                 .Select(c => new DO_BusinessCalendarLink
                                 {
                                     CalenderType = c.CalenderType,
                                     StartMonth = c.StartMonth,
                                     CalenderKey = c.CalenderKey,
                                     //YearEndStatus = c.YearEndStatus,
                                     ActiveStatus = c.ActiveStatus
                                 }).ToListAsync();
                    List<DO_BusinessCalendarLink> lstheader = new List<DO_BusinessCalendarLink>();

                    foreach (var link in result)
                    {
                        var exists = db.GtEcblcls.Where(x => x.CalenderKey == link.CalenderKey && x.BusinessKey == businesskey).FirstOrDefault();
                        if (exists != null)
                        {
                            link.Alreadylinked = true;
                        }
                        else
                        {
                            link.Alreadylinked = false;
                        }
                        DO_BusinessCalendarLink model = new DO_BusinessCalendarLink()
                        {
                            CalenderType = link.CalenderType,
                            StartMonth = link.StartMonth,
                            CalenderKey = link.CalenderKey,
                            YearEndStatus = link.YearEndStatus,
                            ActiveStatus = link.ActiveStatus,
                            Alreadylinked = link.Alreadylinked
                        };
                        lstheader.Add(model);
                    }
                    return lstheader;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertBusinessKeyIntoCalendar(DO_BusinessCalendarLink obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {

                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {


                        var cblink = db.GtEcblcls.Where(x => x.BusinessKey == obj.BusinessKey && x.CalenderKey.ToUpper().Replace(" ", "") == obj.CalenderKey.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (cblink != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0061", Message = string.Format(_localizer[name: "W0061"]) };
                        }
                        GtEcblcl calblink = new GtEcblcl()
                        {
                            BusinessKey = obj.BusinessKey,
                            CalenderKey = obj.CalenderKey,
                            YearEndStatus = obj.YearEndStatus,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEcblcls.Add(calblink);
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };

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

        #region Document Calendar Business Link

        //public async Task<List<DO_BusinessCalendar>> GetBusinesslinkedCalendarkeys(int businessKey)
        //{
        //    try
        //    {
        //        using (eSyaEnterprise db = new eSyaEnterprise())
        //        {

        //            var result = await db.GtEcblcls.Where(x => x.ActiveStatus && x.BusinessKey == businessKey)
        //                .Select(x => new DO_BusinessCalendar
        //                {
        //                    CalenderKey = x.CalenderKey
        //                }).ToListAsync();
        //            var Distinctcals = result.GroupBy(x => x.CalenderKey).Select(y => y.First());
        //            return Distinctcals.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<List<DO_DocumentControl>> GetActiveDocuments()
        //{
        //    try
        //    {
        //        using (eSyaEnterprise db = new eSyaEnterprise())
        //        {

        //            var result = db.GtDccnsts.Where(x => x.ActiveStatus)
        //                .Select(x => new DO_DocumentControl
        //                {
        //                    DocumentId = x.DocumentId,
        //                    DocumentDesc = x.DocumentDesc
        //                }).ToListAsync();

        //            return await result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<List<DO_BusinessCalendar>> GetBusinessCalendarBYBusinessKey(int businessKey)
        //{
        //    try
        //    {
        //        using (eSyaEnterprise db = new eSyaEnterprise())
        //        {

        //            var result = await db.GtDncnbcs.Where(x => x.BusinessKey == businessKey)
        //                .Join(db.GtDccnsts,
        //                dl => dl.DocumentId,
        //                dm => dm.DocumentId,
        //                (dl, dm) => new { dl, dm })
        //                .Select(x => new DO_BusinessCalendar
        //                {
        //                    BusinessKey = x.dl.BusinessKey,
        //                    //CalenderKey = x.dl.CalenderKey,
        //                    DocumentId = x.dl.DocumentId,
        //                    //GeneNoYearOrMonth = x.dl.GeneNoYearOrMonth,
        //                    DocumentDesc = x.dm.DocumentDesc,
        //                    ActiveStatus = x.dl.ActiveStatus,

        //                }
        //                ).ToListAsync();
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<DO_ReturnParameter> InsertOrUpdateBusinessCalendar(DO_BusinessCalendar obj)
        //{
        //    using (eSyaEnterprise db = new eSyaEnterprise())
        //    {
        //        using (var dbContext = db.Database.BeginTransaction())
        //        {
        //            try
        //            {

        //                var bcExist = db.GtDncnbcs.Where(w => w.DocumentId == obj.DocumentId && w.BusinessKey == obj.BusinessKey ).FirstOrDefault();
        //                if (bcExist != null)
        //                {

        //                    bcExist.BusinessKey = obj.BusinessKey;
        //                    //bcExist.CalenderKey = obj.CalenderKey;
        //                    bcExist.DocumentId = obj.DocumentId;
        //                    //bcExist.GeneNoYearOrMonth = obj.GeneNoYearOrMonth;
        //                    bcExist.ActiveStatus = obj.ActiveStatus;
        //                    bcExist.ModifiedBy = obj.UserID;
        //                    bcExist.ModifiedOn = DateTime.Now;
        //                    bcExist.ModifiedTerminal = obj.TerminalID;
        //                    await db.SaveChangesAsync();
        //                    dbContext.Commit();
        //                    return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
        //                }
        //                else
        //                {

        //                    var bc = new GtDncnbc
        //                    {
        //                        BusinessKey = obj.BusinessKey,
        //                        CalendarKey=obj.CalenderKey,
        //                        DocumentId = obj.DocumentId,
        //                        //GeneNoYearOrMonth = obj.GeneNoYearOrMonth,
        //                        ActiveStatus = obj.ActiveStatus,
        //                        //FormId = obj.FormID,
        //                        CreatedBy = obj.UserID,
        //                        CreatedOn = DateTime.Now,
        //                        CreatedTerminal = obj.TerminalID
        //                    };
        //                    db.GtDncnbcs.Add(bc);
        //                    await db.SaveChangesAsync();
        //                    dbContext.Commit();
        //                    return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
        //                }

        //            }
        //            catch (DbUpdateException ex)
        //            {
        //                dbContext.Rollback();
        //                throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
        //            }
        //        }
        //    }
        //}

        #endregion
    }
}
