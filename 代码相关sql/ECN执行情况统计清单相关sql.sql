--ECNִ�����ͳ���嵥���sql
--ָ���ֶι���sql
 select distinct cen.�������� from SelectCENDataList cen 

 --��ѯ�б�sql
 select top 200 num,MainID,���̰汾��,���̱��,��������,���̱���,��Ʒ�ͺ�,��Ʒ����,��������,��������,�����������,����״̬,����ʱ��,����ʱ��,case when �����ڵ� = 'QDȷ����Ϣ' then 'QDȷ����Ϣ'
                  when �����ڵ� = 'MD����' then 'MD����'
                  when �����ڵ� = '�ɹ�����' then 'CM_CPȷ����Ϣ'
                  when �����ڵ� = 'CM_CPȷ����Ϣ' then 'CM_CPȷ����Ϣ'
                  when �����ڵ� = 'MCȷ��' then 'MCȷ��'
                  when �����ڵ� = 'PCȷ��' then 'PCȷ��'
                  when �����ڵ� = 'PC����1' then 'PCȷ��'
                  when �����ڵ� = 'PC����' then 'PCȷ��'
                  when �����ڵ� = 'RDȷ����Ϣ' then 'RDȷ����Ϣ'
                  when �����ڵ� = 'RD��Ա' then 'RD��Ա'
                  when �����ڵ� = 'RD����ʦ' then 'RD����ʦ'
                  when �����ڵ� = 'RD����ʦ1' then 'RD�����ʩ'
                  when �����ڵ� = 'RD�����ʩ' then 'RD�����ʩ'
                  when �����ڵ� = 'RDȷ����Ϣ' then 'RDȷ����Ϣ'
                  when �����ڵ� = 'RD����������' then 'RD����������'
                  when �����ڵ� = 'MC����1' then 'PMC����'
                  when �����ڵ� = 'MC����' then 'PMC����'
                  when �����ڵ� = 'PMC����' then 'PMC����'
                  when �����ڵ� = 'EM����' then 'EM����'
                  when �����ڵ� = '���湤��ʦȷ����Ϣ' then '��������'
                  when �����ڵ� = '��������' then '��������'
                  when �����ڵ� = 'ED����1' then 'ED����'
                  when �����ڵ� = 'ED����' then 'ED����'
                  when �����ڵ� = 'ED����' then 'ED����'
                  when �����ڵ� = 'QD����' then 'QD����'
                  when �����ڵ� = 'PM�ж�' then 'PM�ж�'
                  when �����ڵ� = 'MD����' then 'MD����'
                  when �����ڵ� = 'MD����' then 'MD����'
                  when �����ڵ� = 'RD����2' then 'RD��������'
                  when �����ڵ� = 'RD��������' then 'RD��������'
                  when �����ڵ� = 'RD����1' then 'RD��������'
                  when �����ڵ� = 'RD����3' then 'RD��������'
                  when �����ڵ� = 'RD����' then 'RD��������'
                  when �����ڵ� = 'ECR������ž�������' then 'ECR������ž�������'
                  when �����ڵ� = 'RDָ����Ա��ӡ' then 'RDָ����Ա��ӡ'
                  when �����ڵ� = '������' then '������'
                  else 'δ֪'
                 end �����ڵ� ,��������,������,��������ʱ�� from SelectCENDataList cen where cen.�����ڵ� is not null and cen.�����ڵ�<>'δ֪'  
				  order by cen.���̱��,cen.��������




--�����˵�-�����б��ѯ
select reverse(substring( reverse(filename),0,charindex('\',reverse(filename),0))) as filename,filepos, attachid,djbh from FC_ATTACH 
where djbh='JHC00036958'
order by djbh

select distinct MainID from SelectCENDataList cen where cen.�����ڵ� is not null and cen.�����ڵ�<>'δ֪' and ���̱��='GIPECR20180316002'

select reverse(substring( reverse(filename),0,charindex('\',reverse(filename),0))) as filename,filepos, attachid from FC_ATTACH where djbh='JHC00036972'


	

	select * from SelectCENDataList cen where  cen.���̱��='GIPECR20180516001' ORDER BY cen.���̱��,cen.��������


	select * from SelectCENDataList cen where cen.���̱��<>'' and cen.����״̬<>0 
and cen.���̱��='GIPECR20180124002'
ORDER BY cen.���̱��,cen.�������� 


select distinct MainID from SelectCENDataList cen where cen.�����ڵ� is not null and cen.�����ڵ�<>'δ֪' and ���̱��='GIPECR20180316001'

select distinct MainID from SelectCENDataList cen where cen.�����ڵ� is not null and cen.�����ڵ�<>'δ֪' and ���̱��='GIPECR20180316002'


select reverse(substring( reverse(filename),0,charindex('\',reverse(filename),0))) as filename,filepos, attachid from FC_ATTACH where djbh='JHC00036958'

select  num,MainID,���̱��,���̱���,��Ʒ�ͺ�,��Ʒ����,��������,��������,�����������,����״̬,����ʱ��,����ʱ��,case when �����ڵ� = 'QDȷ����Ϣ' then 'QDȷ����Ϣ'
                  when �����ڵ� = 'MD����' then 'MD����'
  
                  when �����ڵ� = '�ɹ�����' then 'CM_CPȷ����Ϣ'
                  when �����ڵ� = 'CM_CPȷ����Ϣ' then 'CM_CPȷ����Ϣ'

                  when �����ڵ� = 'MCȷ��' then 'MCȷ��'
                  when �����ڵ� = 'PCȷ��' then 'PCȷ��'
                  when �����ڵ� = 'PC����1' then 'PCȷ��'
                  when �����ڵ� = 'PC����' then 'PCȷ��'
  

                  when �����ڵ� = 'RDȷ����Ϣ' then 'RDȷ����Ϣ'
                  when �����ڵ� = 'RD��Ա' then 'RD��Ա'
                  when �����ڵ� = 'RD����ʦ' then 'RD����ʦ'
                  when �����ڵ� = 'RD����ʦ1' then 'RD�����ʩ'
                  when �����ڵ� = 'RD�����ʩ' then 'RD�����ʩ'
                  when �����ڵ� = 'RDȷ����Ϣ' then 'RDȷ����Ϣ'
                  when �����ڵ� = 'RD����������' then 'RD����������'

                  when �����ڵ� = 'MC����1' then 'PMC����'
                  when �����ڵ� = 'MC����' then 'PMC����'
                  when �����ڵ� = 'PMC����' then 'PMC����'

                  when �����ڵ� = 'EM����' then 'EM����'

                  when �����ڵ� = '���湤��ʦȷ����Ϣ' then '��������'
                  when �����ڵ� = '��������' then '��������'

                  when �����ڵ� = 'ED����1' then 'ED����'
                  when �����ڵ� = 'ED����' then 'ED����'
                  when �����ڵ� = 'ED����' then 'ED����'

                  when �����ڵ� = 'QD����' then 'QD����'

                  when �����ڵ� = 'PM�ж�' then 'PM�ж�'

                  when �����ڵ� = 'MD����' then 'MD����'
                  when �����ڵ� = 'MD����' then 'MD����'

                  when �����ڵ� = 'RD����2' then 'RD��������'
                  when �����ڵ� = 'RD��������' then 'RD��������'
                  when �����ڵ� = 'RD����1' then 'RD��������'
                  when �����ڵ� = 'RD����3' then 'RD��������'
                  when �����ڵ� = 'RD����' then 'RD��������'

                  when �����ڵ� = 'ECR������ž�������' then 'ECR������ž�������'
  
                  when �����ڵ� = 'RDָ����Ա��ӡ' then 'RDָ����Ա��ӡ'
                  when �����ڵ� = '������' then '������'
                  else 'δ֪'
                 end �����ڵ� ,��������,������,��������ʱ�� from SelectCENDataList cen where cen.�����ڵ� is not null and cen.�����ڵ�<>'δ֪' 
				 and cen.���̱��='GIPECR20180504001'
                  order by cen.���̱��,cen.��������


--������
--��������
SELECT distinct Domin_Fty FROM c6.dbo.CustomModule_201552114222962 where Domin_Fty<>''
--WHERE Domin_Fty='HL'
--��������
SELECT distinct Produce_Plant FROM c6.dbo.CustomModule_201552114222962 where Produce_Plant<>''
--WHERE Produce_Plant='HL/KS'





