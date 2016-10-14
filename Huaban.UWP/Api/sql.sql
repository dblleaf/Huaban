select top 100 
std_contract.mgr_branch_no,
std_contract.pol_code,
std_contract.cg_no,
std_contract.in_force_date,
std_contract.appl_date,
std_contract.sales_channel,
std_contract.n_sales_code,
std_cpnst_opsn.opsn_name,
std_cpnst_opsn.opsn_sex,
std_cpnst_opsn.opsn_birth_date,
std_cpnst_opsn.opsn_id_type,
std_cpnst_opsn.opsn_id_no,
std_cpnst_opsn.opsn_cust_no,
std_cpnst_opsn.occur_1cls_code,
std_cpnst_opsn.occur_time,
std_cpnst_opsn.occur_course,
cpnst_register.claimer_name,
cpnst_register.claimer_tel,
cpnst_register.cpnst_caserpt_no,
cpnst_register.cpnst_appl_seq,
cpnst_register.bat_cpnst_stl_type,
cpnst_register.proc_stat,
cpnst_register.sys_accept_date,
cpnst_register.close_date,
cpnst_register.oclk_clerk_code,
cpnst_register.pclk_clerk_code,
cpnst_register.cclk_clerk_code,
cpnst_register.sclk_clerk_code,
cpnst_register.eclk_clerk_code,
mio_log.mio_cust_name,
mio_log.bank_acc_no,
mio_log.mio_type_code,
mio_log.plnmio_date,
mio_log.mio_date,
pol_lst.pol_name,
cpnst_cal.cfm_cpnst_amnt,
cpnst_cal.cpnst_type_code,
CPNST_CASERPT.Caserpt_Date,
grp_cntr_hldr.name,
sales_branch.sales_branch_name,
sales_psn.sales_psn_name
from grp_cntr_hldr
inner join std_contract on std_contract.cg_id = grp_cntr_hldr.cg_id
left join sales_branch on sales_branch.sales_branch_no = std_contract.mgr_branch_no
left join pol_lst on pol_lst.pol_code = std_contract.pol_code
left join sales_psn on sales_psn.sales_psn_no = std_contract.n_sales_code
inner join cpnst_appl on cpnst_appl.cg_no = std_contract.cg_no
inner join cpnst_register on cpnst_register.case_reg_id = cpnst_appl.case_reg_id
left join plnmio_rec on plnmio_rec.mtn_id = cpnst_register.case_reg_id
left join mio_log on mio_log.mtn_id = cpnst_register.case_reg_id
inner join std_cpnst_opsn on std_cpnst_opsn.rel_cpnst_cr_id = cpnst_register.case_reg_id
inner join cpnst_cal on cpnst_cal.cpnst_appl_id = cpnst_appl.cpnst_appl_id
inner join CPNST_CASERPT on CPNST_CASERPT.CPNST_CASERPT_ID = std_cpnst_opsn.std_cpnst_opsn_id
where cpnst_register.sys_accept_date >= to_date('20130101', 'yyyy-mm-dd')
and std_cpnst_opsn.rel_cpnst_cr_type = 'R'
and cpnst_register.proc_stat = 'E'
