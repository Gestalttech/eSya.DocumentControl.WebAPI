﻿using eSya.DocumentControl.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.DocumentControl.IF
{
    public interface IBusinessCalendarRepository
    {
        #region Calendar Business Link
        Task<List<DO_BusinessCalendarLink>> GetCalendarHeaders(int businesskey);
        Task<DO_ReturnParameter> InsertBusinessKeyIntoCalendar(DO_BusinessCalendarLink obj);
        #endregion

        #region Document Calendar Business Link
        //Task<List<DO_BusinessCalendar>> GetBusinesslinkedCalendarkeys(int businessKey);

        //Task<List<DO_DocumentControl>> GetActiveDocuments();

        //Task<List<DO_BusinessCalendar>> GetBusinessCalendarBYBusinessKey(int businessKey);

        //Task<DO_ReturnParameter> InsertOrUpdateBusinessCalendar(DO_BusinessCalendar obj);
        #endregion
    }
}
