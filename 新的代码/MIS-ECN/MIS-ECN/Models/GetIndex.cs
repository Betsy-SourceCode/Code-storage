using MIS_ECN.Models.PublicSqlMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIS_ECN.Models
{
    public class GetIndex
    {
        C6Entities db = new C6Entities();
        /// <summary>
        /// 查询首页列表SQL
        /// </summary>
        /// <param name="list"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<SelectCENDataList> GetIndexListSql(SelectCENDataList list, string StartDate, string EndDate)
        {
            try
            {
                string sql = @"select num,MainID,流程版本号,流程编号,申请日期,流程标题,产品型号,产品代码,主导工厂,生产工厂,申请更改内容,流程状态,接收时间,审批时间,case when 审批节点 = 'QD确认信息' then 'QD确认信息'
                  when 审批节点 = 'MD评审' then 'MD评审'
                  when 审批节点 = '采购报数' then 'CM_CP确认信息'
                  when 审批节点 = 'CM_CP确认信息' then 'CM_CP确认信息'
                  when 审批节点 = 'MC确认' then 'MC确认'
                  when 审批节点 = 'PC确认' then 'PC确认'
                  when 审批节点 = 'PC经理1' then 'PC确认'
                  when 审批节点 = 'PC经理' then 'PC确认'
                  when 审批节点 = 'RD确认信息' then 'RD确认信息'
                  when 审批节点 = 'RD文员' then 'RD文员'
                  when 审批节点 = 'RD工程师' then 'RD工程师'
                  when 审批节点 = 'RD工程师1' then 'RD处理措施'
                  when 审批节点 = 'RD处理措施' then 'RD处理措施'
                  when 审批节点 = 'RD确认信息' then 'RD确认信息'
                  when 审批节点 = 'RD负责人审批' then 'RD负责人审批'
                  when 审批节点 = 'MC经理1' then 'PMC审批'
                  when 审批节点 = 'MC经理' then 'PMC审批'
                  when 审批节点 = 'PMC审批' then 'PMC审批'
                  when 审批节点 = 'EM审批' then 'EM审批'
                  when 审批节点 = '安规工程师确认信息' then '安规审批'
                  when 审批节点 = '安规审批' then '安规审批'
                  when 审批节点 = 'ED经理1' then 'ED审批'
                  when 审批节点 = 'ED经理' then 'ED审批'
                  when 审批节点 = 'ED审批' then 'ED审批'
                  when 审批节点 = 'QD审批' then 'QD审批'
                  when 审批节点 = 'PM判断' then 'PM判断'
                  when 审批节点 = 'MD审批' then 'MD审批'
                  when 审批节点 = 'MD经理' then 'MD审批'
                  when 审批节点 = 'RD经理2' then 'RD经理审批'
                  when 审批节点 = 'RD经理审批' then 'RD经理审批'
                  when 审批节点 = 'RD经理1' then 'RD经理审批'
                  when 审批节点 = 'RD经理3' then 'RD经理审批'
                  when 审批节点 = 'RD经理' then 'RD经理审批'
                  when 审批节点 = 'ECR提出部门经理审批' then 'ECR提出部门经理审批'
                  when 审批节点 = 'RD指定人员打印' then 'RD指定人员打印'
                  when 审批节点 = '申请人' then '申请人'
                  else '未知'
                 end 审批节点 ,审批步骤,审批人,审批耗用时间 from SelectCENDataList cen where cen.审批节点 is not null and cen.审批节点<>'未知' ";
                if (list.流程标题 != null)
                {
                    sql = sql + " AND 流程标题 LIKE '%" + list.流程标题 + "%' ";
                }
                if (list.流程编号 != null)
                {
                    sql = sql + " AND 流程编号 LIKE '%" + list.流程编号 + "%' ";
                }
                if (StartDate != "")
                {
                    sql = sql + " AND 申请日期 >='" + StartDate + "' ";
                }
                if (EndDate != "")
                {
                    sql = sql + " AND 申请日期 <='" + EndDate + "' ";
                }
                if (list.产品型号 != null)
                {
                    sql = sql + " AND 产品型号 LIKE '%" + list.产品型号 + "%' ";
                }
                if (list.产品代码 != null)
                {
                    sql = sql + " AND 产品代码 LIKE '%" + list.产品代码 + "%' ";
                }
                if (list.生产工厂 != null) //下拉框
                {
                    sql = sql + " AND 生产工厂 = '" + list.生产工厂 + "'";
                }
                if (list.主导工厂 != null) //下拉框
                {
                    sql = sql + " AND 主导工厂= '" + list.主导工厂 + "'";
                }
                //if (list.流程状态 != -1)  //-1代表全部
                //{
                //    sql = sql + " AND 流程状态 =" + list.流程状态 + "";
                //}
                sql = sql + "  order by cen.流程编号 desc,cen.审批步骤";
                List<SelectCENDataList> DataList = db.Database.SqlQuery<SelectCENDataList>(sql).ToList();
                List<SelectCENDataList> DataList1 = new List<SelectCENDataList>();



                for (int i = 0; i < DataList.Count; i++)
                {
                    if (i < DataList.Count - 1)
                    {
                        if (DataList[i].流程状态 != 0)
                        {
                            DataList1.Add(DataList[i]);
                        }
                        else if (DataList[i].流程编号 != DataList[i + 1].流程编号)
                        {
                            DataList1.Add(DataList[i]);
                        }
                    }
                    else
                    {
                        DataList1.Add(DataList[i]);
                    }

                }

                return DataList1;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                throw;
            }

        }

        /// <summary>
        /// 二级菜单文件路径列表SQL
        /// </summary>
        /// <param name="MainID"></param>
        /// <returns></returns>
        public List<Download> ByMainGetDjbhNameSql(string MainId)
        {
            string sql = string.Format(@"select reverse(substring( reverse(filename),0,charindex('\',reverse(filename),0))) as filename,filepos, attachid from FC_ATTACH where djbh='{0}'", MainId);
            List<Download> DataList = db.Database.SqlQuery<Download>(sql).ToList();
            return DataList;
        }

        /// <summary>
        /// 通过流程编号找到对应的MainIdSQL
        /// </summary>
        /// <param name="CERNumber"></param>
        /// <returns></returns>
        public string GetMainIdSql(string CERNumber)
        {
            string sql = string.Format(@"select distinct MainID from SelectCENDataList cen where cen.审批节点 is not null and cen.审批节点<>'未知' and 流程编号='{0}'", CERNumber);
            string DataList = db.Database.SqlQuery<string>(sql).FirstOrDefault();
            return DataList;
        }

        /// <summary>
        /// 获得获得下拉框列表-生产工厂SQL
        /// </summary>
        /// <returns></returns>
        public List<string> GetProducePlantDropSql()
        {
            try
            {
                List<string> list = db.Database.SqlQuery<string>("SELECT distinct Produce_Plant FROM c6.dbo.CustomModule_201552114222962 where Produce_Plant<>''").ToList<string>();
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
                return null;
            }

        }
        /// <summary>
        /// 获得获得下拉框列表-生产工厂SQL
        /// </summary>
        /// <returns></returns>
        public List<string> GetDominFtyDropSql()
        {
            try
            {
                List<string> list = db.Database.SqlQuery<string>("SELECT distinct Domin_Fty FROM c6.dbo.CustomModule_201552114222962 where Domin_Fty<>''").ToList<string>();
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
                return null;
            }
        }

        public int GetWeekendSql(string submitTime, string approveTime)
        {
            string sql = "SELECT  dbo.F_btTimes('{0}','{1}') 工作耗时";
            try
            {
                
                sql = string.Format(sql, submitTime, approveTime);
                int Data = db.Database.SqlQuery<int>(sql).FirstOrDefault();
                return Data;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message + "错误sql为：" + sql);
                throw;
            }

        }
    }
}