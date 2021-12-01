using KYH_ProductPrice.Models.PublicSqlMethods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KYH_ProductPrice.Models.SqlStatement
{
    // Token: 0x0200000E RID: 14
    public class PublicSqlMethodsSql
    {
        public static string sql;
        private WebStationEntities db = new WebStationEntities();
        /// <summary>
        /// 获得详情页面列表SQL(带条件)
        /// </summary>
        /// <param name="CreateBy"></param>
        /// <param name="CustProd"></param>
        /// <param name="Customer"></param>
        /// <param name="CreateTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Remarks_MD"></param>
        /// <param name="Cancel"></param>
        /// <param name="username"></param>
        /// <param name="login_Dept"></param>
        /// <returns></returns>
        public List<GetProductPriceListss> GetDetailsListSql(string CreateBy, string CustProd, string Customer, string CreateTime, string EndTime, string Remarks_MD, bool Cancel, string username, string login_Dept, string Rank,string ZT)
        {
            try
            {
                sql = "Set Language US_English";
                sql = sql+ " SELECT A.CreateDept+D.UpperDept AS upDepartment, A.CPSerial , REPLACE(CONVERT(VARCHAR(11),A.CreateTime,113),' ','-') + ' ' + RIGHT(CONVERT(varchar(20),A.CreateTime, 113), 8) CreateTime, C.AliasName + ' ' + C.LastName + ' (' + D.DeptCode + ')' CreateBy, A.Ledger, ISNULL(F.FullName_CN, F.FullName_EN) + ' (' + F.CountryID + '-' + G.CityName_CN + ')' CustomerDisplayName,A.FNumber, A.CustProdCode, A.CustProdName, A.PrvCurr, A.PrvUnit, A.UpdCurr, A.UpdUnit, E.Name_EN EffType, A.EffDetail, A.Remarks_MD, A.Remarks_FD, A.Cancel";
                sql = sql+ " FROM CustProductPriceRecords A LEFT OUTER JOIN Customer B ON A.CustomerID = B.CustomerID LEFT OUTER JOIN Employee C ON A.CreateBy = C.EmpID LEFT OUTER JOIN Department D ON C.DeptID = D.DeptID LEFT OUTER JOIN TBWords E ON A.EffType = E.Value AND E.WordCode = 'EF' LEFT OUTER JOIN Company F ON B.SysID = F.CompanyID LEFT OUTER JOIN City AS G ON F.CityID = G.CityID";
                sql = sql+ " WHERE 1=1 ";
                if (CreateBy!=null) //业务员文本框的值
                {
                    sql =sql+ " AND (C.AliasName + ' ' + C.LastName) LIKE '%" + CreateBy + "%' ";
                }
                if (ZT!="ALL")
                {
                    sql = sql+ " AND A.Ledger ='"+ZT+"'";
                }
                if (Customer!=null)
                { 
                    sql =sql+ " AND ISNULL(F.FullName_CN,'') + '|' + ISNULL(F.FullName_EN,'') + '|' + F.ShortName like '%" + Customer + "%'";
                }
                if (CustProd!="")
                {
                    sql =sql+ " AND A.FNumber + '|' + A.CustProdCode + '|' + A.CustProdName LIKE '%" + CustProd + "%' ";
                }
                if (CreateTime!=null)
                {
                    sql =sql+ " AND (DATEDIFF(d,'" + CreateTime +"',A.CreateTime) >= 0) ";
                }
                if (EndTime!="")
                {
                    sql =sql+" AND (DATEDIFF(d,'" +EndTime +"',A.CreateTime) <= 0)";
                }
                if (Cancel==true)
                {
                    sql =sql+ " AND A.Cancel = 1";
                }
                if (Remarks_MD!=null)
                {
                    sql = sql+ " AND  (ISNULL(A.Remarks_MD, N'') + '|' + ISNULL(A.Remarks_FD, N'') + '|' + A.EffDetail LIKE '%" + Remarks_MD+ "%')";
                }
                string department = new Authority().GetDepartmentSql(username, 0);
                if (login_Dept != "ACC")
                {
                    sql = sql + " AND (A.CreateDept+D.UpperDept LIKE '" + department + "%')";
                    //if (IsUpDepartment(department) > 0)
                    //{
                    //    sql = sql + " AND ( (A.CreateDept+D.UpperDept) LIKE '%" + department + "%')";
                    //}
                    //else
                    //{
                    //    sql = sql + " AND  (B.ServiceBy LIKE '%|" + username + "|%')";
                    //}
                }
               
                sql = sql+ " order by " + Rank;
                // sql = @"SELECT * FROM GetProductPriceList WHERE  
                //        (FNumber LIKE '%{1}%' OR CustProdCode LIKE '%{1}%' OR CustProdName LIKE '%{1}%') 
                //        AND CustomerDisplayName  LIKE '%{2}%' AND (CreateTime >= '{3}' AND CONVERT(NVARCHAR(10),CreateTime,23) <= '{4}') 
                //        AND (EffDetail like  '%{5}%' or Remarks_MD like  '%{5}%' or  Remarks_FD like  '%{5}%')  AND  Ledger like '%{6}%'";
                //if (username==null && login_Dept==null)
                //{
                //    sql += "AND CreateBy like '%{0}%'";
                //}
                //if (login_Dept != null)
                //{
                //    sql = string.Concat(new string[]
                //    {
                //        sql,"  AND( CreateDept+UpperDept LIKE '%",login_Dept,"%'  or   CreateBy='",username,"')"
                //    });
                //}
                //else if (username != null)
                //{
                //    if (CreateBy !=null)
                //    {
                //        sql += "  AND CreateBy like '%" + CreateBy + "%'";
                //    }
                //     sql = sql + "  AND CreateBy='" + username + "'";
                //}
                //if (Cancel)
                //{
                //    sql += "  AND Cancel=1";
                //}
                //string Ordersql = "  Order by  " + Rank; //排序



                //sql = string.Format(sql + Ordersql, new object[]
                //{
                //    CreateBy,
                //    CustProd,
                //    Customer,
                //    CreateTime,
                //    EndTime,
                //    Remarks_MD,
                //    ZT
                //});
                List<GetProductPriceListss> list = this.db.Database.SqlQuery<GetProductPriceListss>(sql, new object[0]).ToList<GetProductPriceListss>();
                return list;
            }
            catch (Exception ex)
            {
                string aa= ex.Message;
                throw;
            }
           
        }/// <summary>
        /// 查询是否存在上级部门
        /// </summary>
        /// <param name="login_Dept">登陆部门</param>
        /// <returns></returns>
        public int IsUpDepartment(string department)
        {
            
            int sql = db.Database.SqlQuery<int>(@"select Count(*) FROM CustProductPriceRecords A LEFT OUTER JOIN Customer B ON 
                            A.CustomerID = B.CustomerID LEFT OUTER JOIN Employee C ON A.CreateBy = C.EmpID LEFT OUTER JOIN Department D 
                            ON C.DeptID = D.DeptID where A.CreateDept+D.UpperDept like '%"+ department + "%' ").FirstOrDefault();
            return sql;
        }
        /// <summary>
        /// 获得详情页面列表SQL(不带条件)
        /// </summary>
        /// <param name="CPSerial"></param>
        /// <returns></returns>
        public List<GetProductPriceList> GetDetailsListSql(int CPSerial)
        {
            string sql = "SELECT * FROM GetProductPriceList list WHERE list.CPSerial=" + CPSerial.ToString();
            return this.db.Database.SqlQuery<GetProductPriceList>(sql, new object[0]).ToList<GetProductPriceList>();
        }
    }
}
