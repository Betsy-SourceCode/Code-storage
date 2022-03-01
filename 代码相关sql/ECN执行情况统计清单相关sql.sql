--ECN执行情况统计清单相关sql
--指定字段过滤sql
 select distinct cen.主导工厂 from SelectCENDataList cen 

 --查询列表sql
 select top 200 num,MainID,流程版本号,流程编号,申请日期,流程标题,产品型号,产品代码,主导工厂,生产工厂,申请更改内容,流程状态,接收时间,审批时间,case when 审批节点 = 'QD确认信息' then 'QD确认信息'
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
                 end 审批节点 ,审批步骤,审批人,审批耗用时间 from SelectCENDataList cen where cen.审批节点 is not null and cen.审批节点<>'未知'  
				  order by cen.流程编号,cen.审批步骤




--二级菜单-下载列表查询
select reverse(substring( reverse(filename),0,charindex('\',reverse(filename),0))) as filename,filepos, attachid,djbh from FC_ATTACH 
where djbh='JHC00036958'
order by djbh

select distinct MainID from SelectCENDataList cen where cen.审批节点 is not null and cen.审批节点<>'未知' and 流程编号='GIPECR20180316002'

select reverse(substring( reverse(filename),0,charindex('\',reverse(filename),0))) as filename,filepos, attachid from FC_ATTACH where djbh='JHC00036972'


	

	select * from SelectCENDataList cen where cen.流程编号<>'' and cen.接收时间>'2017-3-12' and cen.流程编号='GIPECR20171212001' ORDER BY cen.流程编号,cen.审批步骤


	select * from SelectCENDataList cen where cen.流程编号<>'' and cen.流程状态<>0 
and cen.流程编号='GIPECR20180124002'
ORDER BY cen.流程编号,cen.审批步骤 


select distinct MainID from SelectCENDataList cen where cen.审批节点 is not null and cen.审批节点<>'未知' and 流程编号='GIPECR20180316001'

select distinct MainID from SelectCENDataList cen where cen.审批节点 is not null and cen.审批节点<>'未知' and 流程编号='GIPECR20180316002'


select reverse(substring( reverse(filename),0,charindex('\',reverse(filename),0))) as filename,filepos, attachid from FC_ATTACH where djbh='JHC00036958'

select top 200 num,MainID,流程编号,流程标题,产品型号,产品代码,主导工厂,生产工厂,申请更改内容,流程状态,接收时间,审批时间,case when 审批节点 = 'QD确认信息' then 'QD确认信息'
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
                 end 审批节点 ,审批步骤,审批人,审批耗用时间 from SelectCENDataList cen where cen.审批节点 is not null and cen.审批节点<>'未知' 
                  order by cen.流程编号,cen.审批步骤


				  SELECT DISTINCT *,DATEDIFF( Minute,提交时间, 审批时间) AS '审批耗用时间(分钟)'  FROM (
    SELECT  RANK() OVER (PARTITION BY a.FlowNo ORDER BY c.App_Order ) AS Num,a.MainID,  a.FlowNo  '流程编号',c.AppO_Title '流程标题',a.Good_Model '产品型号',a.Good_Num '产品代码',a.Produce_Plant '主导工厂',a.Domin_Fty '生产工厂', a.Change_Details '申请更改内容',c.App_IdeaFlag '流程状态', app_begintime '提交时间',ISNULL(App_Time,GETDATE())  '审批时间',
                d.AppD_Name  '审批节点' ,c.App_Order  '审批步骤',u.UserName  '审批人'
FROM dbo.CustomModule_201552114222962  AS a INNER JOIN
     dbo.JHOA_Approve AS c ON a.MainID = c.AppO_Values
     INNER JOIN dbo.JHOA_Approve_Temp_Dispose AS d ON c.AppD_ID = d.AppD_ID
     LEFT JOIN dbo.users u WITH(NOLOCK) ON c.Reg_Code = u.UserID
WHERE   (c.Del_Flag = '0'AND a.FlowNo='GIPECR20211208002'AND d.appd_name<>'开始')
) a ORDER BY a.流程编号


select * from dbo.CustomModule_201552114222962

