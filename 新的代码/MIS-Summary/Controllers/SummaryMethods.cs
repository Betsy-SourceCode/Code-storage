using MIS_Summary.Models;
using MIS_Summary.Models.PublicSqlMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIS_Summary.Controllers
{
    public class SummaryMethods
    {
      
        public static List<View_Summary> SelectSummaryListSQL(string DocumentNo,string Mat_Code,string Mat_Name,string ApplyStartDate,string ApplyEndDate)
        {
            try
            {
                C6Entities db = new C6Entities();
                //string sql = string.Format("select * FROM View_Summary where (Serial_Num+'|'+OA_number+'|'+PO+'|'+Fixed_number) like '%{0}%' and Mat_Code like '%{1}%' and Mat_Name like '%{2}%' and ApplyDate between '{3}' and '{4}'order by ApplyDate desc, OA_number, PO ", DocumentNo, Mat_Code, Mat_Name, ApplyStartDate, ApplyEndDate); 
                string sql = "select * FROM View_Summary where 1=1";
                if (DocumentNo != "" && DocumentNo != null)
                {
                    sql += "and (Serial_Num + '|' + OA_number + '|' +PO+ '|' + Fixed_number) like '%" + DocumentNo + "%'";
                }
                if (Mat_Code != "" && Mat_Code != null)
                {
                    sql += "and Mat_Code like '%" + Mat_Code + "%'";
                }
                if (Mat_Name != "" && Mat_Name != null)
                {
                    sql += "and Mat_Name like '%" + Mat_Name + "%'";
                }
                if (ApplyStartDate != "" && ApplyStartDate != null && ApplyEndDate != null && ApplyEndDate != "")
                {
                    sql += "and ApplyDate between '" + ApplyStartDate + "' and '" + ApplyEndDate + " '";
                }
                sql += "  order by ApplyDate desc,OA_number,PO";//排列顺序：验收申请日期（倒序）、 采购订单号 、OA申请单号
                return db.Database.SqlQuery<View_Summary>(sql, new object[0]).ToList<View_Summary>();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
        public static List<View_SelectNoApprove> SelectNoApproveListSQL(string Serial_Num)
        {
            try
            {
                C6Entities db = new C6Entities();
                string sql = "select * from View_SelectNoApprove where Serial_Num='" + Serial_Num + "'";

                return db.Database.SqlQuery<View_SelectNoApprove>(sql, new object[0]).ToList<View_SelectNoApprove>();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
    }
}