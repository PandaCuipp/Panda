using System.Text;

namespace CreateNCSql.Code
{
    /// <summary>
    /// 
    /// </summary>
    public class NcStep
    {
        /// <summary>
        /// 发票保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FaPiaoSave(InputDto input)
        {
            var sb = new StringBuilder();

            // 
            sb.AppendLine($@"
-- 保存发票
INSERT INTO po_invoice ( 
cinvoiceid, pk_corp, vinvoicecode, iinvoicetype, cdeptid, cfreecustid, cvendormangid, cvendorbaseid, cemployeeid
, dinvoicedate, darrivedate, cbiztype, caccountbankid, cpayunit, finitflag, cvoucherid, ctermprotocolid, coperator
, caccountyear, cbilltype, ibillstatus, dauditdate, cauditpsn, vmemo, vdef1, vdef2, vdef3, vdef4, vdef5, vdef6
, vdef7, vdef8, vdef9, vdef10, vdef11, vdef12, vdef13, vdef14, vdef15, vdef16, vdef17, vdef18, vdef19, vdef20
, pk_defdoc1, pk_defdoc2, pk_defdoc3, pk_defdoc4, pk_defdoc5, pk_defdoc6, pk_defdoc7, pk_defdoc8, pk_defdoc9
, pk_defdoc10, pk_defdoc11, pk_defdoc12, pk_defdoc13, pk_defdoc14, pk_defdoc15, pk_defdoc16, pk_defdoc17
,pk_defdoc18, pk_defdoc19, pk_defdoc20, dr, cstoreorganization, pk_purcorp, tmaketime, taudittime, tlastmaketime
, invoicenum, bjtgyflag ,ts
) values 
( 
'100966100000000SLB1G'  -- cinvoiceid 主键 发票ID
, '1009'
, 'My_FaPiaoWeiHu_0128001'
, '0'
, '1009A8100000000000RL', NULL, '0001O110000000020Q6S', '0001O1100000000209N1'
, '1002O11000000002JQYD', '2021-01-28', '2021-01-28', '0001O110000000001GTR'
, NULL, '0001O110000000020Q6S', '0', NULL, NULL, '000166100000001HGVJC'
, '2021', '25', '0', NULL, NULL, '备注', NULL, NULL, NULL, NULL, NULL, NULL
, '500.00', '4', '销售部', '深圳', '报销事由_0128001，2938元', '2938.00'
, NULL, NULL, NULL, NULL, '是', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
, NULL, '0001O11000000013MKEG', '0001O1100000000FMC20', NULL, NULL, NULL, NULL, NULL, NULL
, '0001O11000000002S8KM', NULL, NULL, NULL, '0', '1009O1100000000000T0', '1009', '2021-02-01 11:25:04'
, NULL, '2021-02-01 11:25:04', 'My_fph_0128001', 'N', '2021-02-01 11:25:05'

);

");

            return sb.ToString();
        }

        /// <summary>
        /// 应付单审核
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string YFApproval(InputDto input)
        {
            var sb = new StringBuilder();
            //更新主表状态
            var dateStr = input.BillDate.ToString("yyyy-MM-dd");

            sb.AppendLine($@"
-- 应付单审核
update arap_djzb set ts = '{GetDateTimeString()}'
, sxbz = 10
, sxr = '{input.Creator}'
, sxkjnd = '{input.BillDate:yyyy}'
, sxkjqj = '{input.BillDate:MM}'
, sxrq = '{dateStr}' 
where vouchid = '{input.YFVouchid}';

update arap_djzb set ts = '{GetDateTimeString()}'
, spzt = '1'
, djzt = 2
, sxbz = 10
, shr = '{input.Creator}'
, shkjnd = '{input.BillDate:yyyy}'
, shkjqj = '{input.BillDate:MM}'
, shrq = '{dateStr}'
, sxr = '{input.Creator}'
, sxkjnd = '{input.BillDate:yyyy}'
, sxkjqj = '{input.BillDate:MM}'
, sxrq = '{dateStr}' 
where vouchid = '{input.YFVouchid}' and djzt = 1;

");

            return sb.ToString();

            input.DapRtvouchKey = $"{input.DwbmParentId}6610000000{input.RandNo}CF";
            //生成 实时凭证
            sb.AppendLine($@"
-- 生成 实时凭证
insert into 
dap_rtvouch(
pk_voucher, pk_corp, year, period, type, no, prepareddate, tallydate,  attachment, prepared, checker, casher
, manager, signflag, headmodflag, discardflag, sysid,  addclass, deleteclass , assino,pk_glorg,pk_glbook
, def1,def2,def3,def4,def5,def6,def7,def8,def9,def10,freevalue1,freevalue2,freevalue3,freevalue4,freevalue5
,voucherkind,ts
) 
values(
'{input.DapRtvouchKey}'                 -- pk_voucher 实时凭证主键 102266100000000VDLJU
,'{input.Dwbm}'                         -- pk_corp  单位编码 1009
,'{input.BillDate.Year}'                -- year 会计年度
,'{input.BillDate:MM}'                  -- period 会计期间 
,'0001O110000000020ZN7'                 -- type 凭证类别 0001O110000000020ZN3   0001O110000000020ZN7
, 0                                     -- no  凭证编码 integer
, '{input.BillDate:yyyy-MM-dd}'         -- prepareddate 制单日期
, NULL                                  -- tallydate 记帐日期
,{input.DanJuNum}                       -- attachment 附单据数
,NULL                                   -- prepared 制单人
,NULL                                   -- checker 审核人
,NULL                                   -- casher 出纳
,NULL                                   -- manager 记帐人
,NULL                                   -- signflag 签字标志
,'YYYYYYYYYYYYYYY'                      -- headmodflag 修改标志
,NULL                                   -- discardflag 作废标志
,'ARAP'                                 -- sysid 制单系统
,NULL                                   -- addclass 增加接口类
,NULL                                   -- deleteclass  删除接口类
,NULL                                   -- assino 业务关联号
,'{input.GlorgKey}'                     -- pk_glorg 会计主体
,'{input.GlbookKey}'                    -- pk_glbook 财务账簿
, def1,def2,def3,def4,def5,def6,def7,def8,def9,def10,freevalue1,freevalue2,freevalue3,freevalue4,freevalue5
,0                                      -- voucherkind 凭证类型
,'{GetDateTimeString()}'                -- ts
);
");

            // 实时凭证明细
            var fuzhuID = $"1009O110000000003VI1";
            for (int i = 0; i < input.GoodsList.Count; i++)
            {
                var item = input.GoodsList[i];
                var detailId1 = $"{input.DwbmParentId}6610000000{input.RandNo + i}BE";
                var detailId2 = $"{input.DwbmParentId}6610000000{input.RandNo + i}BD";
                var detailId3 = $"{input.DwbmParentId}6610000000{input.RandNo + i}BC";

                var iindex = i + 1;
                sb.AppendLine($@"
-- 实时凭证明细
insert into 
dap_rtvouch_b(

pk_detail, pk_voucher, pk_corp, iindex, fk_accsubj, id, explanation, no,opposingsubj, price, excrate1, excrate2, debitaquality
, debitamount, fracdebitamount, localdebitamount, creditquality, creditamount, fraccreditamount, localcreditamount, modifyflag
, reciepclass, pk_currtype
-- , fk_jsfs, checkno, checkdate,pk_innercorp
-- ,def1,def2,def3,def4,def5,def6,def7,def8,def9,def10,def11
-- ,def12,def13,def14,def15,def16,def17,def18,def19,def20,def21,def22,def23,def24,def25,def26,def27,def28,def29,def30,freevalue1
-- ,freevalue2,freevalue3,freevalue4,freevalue5,freevalue6,freevalue7,freevalue8,freevalue9,freevalue10,freevalue11,freevalue12
-- ,freevalue13,freevalue14,freevalue15,freevalue16,freevalue17,freevalue18,freevalue19,freevalue20,freevalue21,freevalue22
-- ,freevalue23,freevalue24,freevalue25,freevalue26,freevalue27,freevalue28,freevalue29,freevalue30
-- ,tradeno,tradedate ,fk_pjlx,pk_account,pk_contract,startrestdate,assino,coorno,creditsign,bankcode
,pk_glorg,pk_glbook,ts

) values(

'{detailId1}' -- pk_detail 实时凭证分录主键
,'{input.DapRtvouchKey}' -- pk_voucher 实时凭证主键
,'{input.Dwbm}'         -- pk_corp 公司编码
,{iindex} -- iindex 分录号，从1开始
, fk_accsubj  科目编码
, id 辅助核算标识
, explanation
, no,opposingsubj, price, excrate1, excrate2, debitaquality
, debitamount, fracdebitamount, localdebitamount, creditquality, creditamount, fraccreditamount, localcreditamount, modifyflag
, reciepclass, pk_currtype
-- , fk_jsfs, checkno, checkdate,pk_innercorp
-- ,def1,def2,def3,def4,def5,def6,def7,def8,def9,def10,def11
-- ,def12,def13,def14,def15,def16,def17,def18,def19,def20,def21,def22,def23,def24,def25,def26,def27,def28,def29,def30,freevalue1
-- ,freevalue2,freevalue3,freevalue4,freevalue5,freevalue6,freevalue7,freevalue8,freevalue9,freevalue10,freevalue11,freevalue12
-- ,freevalue13,freevalue14,freevalue15,freevalue16,freevalue17,freevalue18,freevalue19,freevalue20,freevalue21,freevalue22
-- ,freevalue23,freevalue24,freevalue25,freevalue26,freevalue27,freevalue28,freevalue29,freevalue30
-- ,tradeno,tradedate ,fk_pjlx,pk_account,pk_contract,startrestdate,assino,coorno,creditsign,bankcode
,pk_glorg,pk_glbook,ts


);
");

            }

            return sb.ToString();
        }

        public string JeiSuan(InputDto input)
        {
            return "";

            #region 付款单结算
            /*
            for (int i = 0; i < goodsList.Count; i++)
            {
                var item = goodsList[i];

                sb.AppendLine($@"
-- 付款单结算 附表 （{i + 1}/{goodsList.Count}）

update arap_djfb set ts='{DateTime.Now:yyyy-MM-dd HH:mm:ss}'
,payman = '{creator}'		-- payman 支付人 000166100000001HGVJC
, paydate = '{date:yyyy-MM-dd}'				-- paydate 支付日期 2021-01-15
, payflag = 1 							-- payflag 支付状态 1
where fb_oid = '{item.FkItemBillNo}';

");
            }

            sb.AppendLine($@"
-- 付款单结算
update arap_djzb set ts='{DateTime.Now:yyyy-MM-dd HH:mm:ss}'
,payman = '{creator}'
, paydate = '{date:yyyy-MM-dd}'
, jszxzf = 3                -- jszxzf 结算中心支付
, zzzt = 1                  -- zzzt 支付状态
where vouchid = '{fk_vouchid}';

");

            #endregion

            #region 添加结算信息

            sb.AppendLine($@"
-- 结算信息 cmp_settlement
INSERT INTO cmp_settlement ( 
iswagelisting, isbusicontrol, pk_finorg, busistatus, settlestatus, arithmetic, effectstatus, busi_billdate
, pk_settlement, isbusieffect, pk_operator, def7, def8, def5, def6, def9, systemcode, pk_tradetype, pk_ftsbill
, pk_executor, lastupdatedate, iscmcandeal, busi_auditdate, settletype, pk_busibill, def20, def4, pk_signer
, def3, def2, def1, def12, def13, def10, pk_auditor, def11, isautosign, isautopay, direction, def18, def19, def16
, def17, def14, def15, isautogenerate, pk_busitype, dr, isapplybill, pk_corp, billcode, payway, signdate, issettleeffect
, setlledate, fts_billtype, setlleno, primal, pk_billoperator, ispay, local, aduitstatus, lastupdater ,ts
) 
VALUES 
( 
NULL,						-- iswagelisting 是否携带工资清单
NULL,						-- isbusicontrol 是否业务控制数据处理
NULL,						-- pk_finorg 财务组织（预留）
4,							-- busistatus 业务单据状态 2,4
5,							-- settlestatus 结算状态 0,5 Update
0,							-- arithmetic 结算信息处理算法
10							-- effectstatus 业务单据生效状态  0,10   Update
,'{dateStr}'				-- busi_billdate 业务单据日期
,'{pk_settlement}'		-- pk_settlement 主键 11076610000000009NTX
,'Y'						-- isbusieffect 业务单据是否已生效 Update
,'{creator}'		-- pk_operator  录入人
,NULL, NULL, NULL, NULL, NULL		-- def7, def8, def5, def6, def9, , 
,'arap'						-- systemcode 归属系统 归属系统
, 'D3'						-- pk_tradetype 业务单单据类型 D3付款单  D1采购应付单
,NULL						-- pk_ftsbill 委托收付款单主键
,'{creator}'		-- pk_executor 结算人 Update
,'{dateStr}'				-- lastupdatedate 最新更新日期
,'Y'						-- iscmcandeal 是否结算平台可处理
,'{dateStr}'				-- busi_auditdate 业务单据审核日期 Update
,0							-- settletype 完成结算方式 0
,'{fk_vouchid}'		-- pk_busibill 付款单 主键 11076610000000009NTT
,NULL, NULL					-- def20, def4
,'{creator}'		-- pk_signer 签字确认人 Update
, NULL, NULL, NULL, NULL, NULL, NULL 		-- def3, def2, def1, def12, def13, def10
,'{creator}'		-- pk_auditor 业务单据审核人
,NULL 						-- def11 自定义项
,'Y'						-- isautosign 是否自动签字
,NULL						-- isautopay 是否自动支付
,1							-- direction 方向
, NULL, NULL, NULL, NULL, NULL, NULL		-- def18, def19, def16, def17, def14, def15
, NULL						-- isautogenerate 是否自动生成
,'0001O110000000001GTR'		-- pk_busitype 单据业务类型 0001O110000000001GTE 一般采购业务；  0001O110000000001GTR 采购补录业务
,0							-- dr 删除标志
,'N'						-- isapplybill 收付申请单
,'{dwbm}'						-- pk_corp 公司pk
, '{fkDanJuHao}'		-- billcode 付款单 业务单据编号 FK***21012500007
, NULL						-- payway  收付款方式
,'{dateStr}'				-- signdate 签字确认日期 Update
,'N'						-- issettleeffect 结算单是否生效
, '{dateStr}'				-- setlledate 结算日期
, NULL						-- fts_billtype 委托收付款单单据类型
, '{setlleno}'		-- setlleno 结算号  Update 
, {taxTotal:F2}						-- primal  原币金额
, '{creator}'	-- pk_billoperator  业务单据录入人
, 'Y'						-- ispay  是否委付
, {taxTotal * huilv:F2}					-- local 本币金额
, 0							-- aduitstatus 业务单据审批状态 0
, '{creator}'	-- lastupdater 最新更新人
, '{GetDateTimeString()}'		-- ts 时间戳

);

");

            for (int i = 0; i < goodsList.Count; i++)
            {
                var item = goodsList[i];

                sb.AppendLine($@"
-- 结算明细 cmp_detail
INSERT INTO cmp_detail ( 
settlelineno, lastest_paydate, isacccord, pk_finorg, oppbank, pk_notetype, pk_account, processtype, expectate_date
, issamebank, def7, def8, def5, def6, def9, systemcode, tallystatus, oppaccount, checkcount, def4, fundformcode
, def3, def2, def1, def12, pk_psndoc, def13, def10, def11, memo, paylocal, def18, def19, def16, def17, def14, def15
, pk_balatype, billtime, checkdate, fracrate, pk_busitype, pk_oppbank, pk_innercorp, bankrelated_code, pk_trader
, dr, transtype, pk_billdetail, tradertype, signdate, pk_cashflow, tradername, pk_job, pk_deptdoc, busilineno
, exectime, tallydate, pk_paybill, settlestatus, pk_rescenter, execdate, pk_currtype, pay_type, signtime, pk_settlement
, pk_bill, pk_billtype, receivelocal, payfrac, code, localrate, modifyflag, pk_jobphase, paybillcode, fundsflag
, def20, bankmsg, notenumber, pk_detail, pk_invcl, pk_plansubj, pk_cubasdoc, notedirection, pk_assaccount, direction
, pk_bank, groupno, pk_oppaccount, pk_invbasdoc, billdate, pay, receivefrac, receive
, pk_costsubj, pk_corp, billcode, issamecity, pk_billbalatype, pk_mngaccount, isbillrecord ,ts
) 
VALUES 
( 
{i} 									-- settlelineno 结算单行号 
, NULL								-- lastest_paydate 最迟支付日期
, 'N'								-- isacccord 是否记账记录
-- pk_finorg, oppbank, pk_notetype, pk_account, processtype, expectate_date, issamebank, def7, def8, def5, def6, def9
, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
, 'arap'							-- systemcode 来源系统
, 4									-- tallystatus 记账状态 1->4
, NULL								-- oppaccount 对方银行账号
, 0									-- checkcount 对帐次数
, NULL								-- def4
, 3									-- fundformcode 资金形态
, NULL, NULL, NULL, NULL			-- def3, def2, def1, def12
, '{ywybm}'			-- pk_psndoc  人员档案
, NULL, NULL, NULL					-- def13, def10, def11
-- memo 摘要
, '{zy}'
, {item.TaxTotal}							-- paylocal 付款本币
, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL		-- def18, def19, def16, def17, def14, def15  pk_balatype 结算方式, billtime 单据时间
, '{dateStr}'						-- checkdate  对账日期
, NULL								-- fracrate 辅币汇率
, '0001O110000000001GTR'			-- pk_busitype 业务类型 0001O110000000001GTE 一般采购业务员；0001O110000000001GTR 采购补录业务 
, '{bankNumber}'			-- pk_oppbank  对方银行主键
, NULL, NULL						-- pk_innercorp 内部单位, bankrelated_code 银行标识码
, '{hbbm}'			-- pk_trader 对方 供应商 主键 0001O11000000009FZVJ
, 0									-- dr 删除标志
, 1									-- transtype 交易种类
, '{item.FkItemBillNo}'			-- pk_billdetail 来源单据行id
, 0									-- tradertype 对方类型
, '{dateStr}'						-- signdate 签字确认日期 Update
, NULL, NULL						-- pk_cashflow 现金流量项目, tradername 对方对象名称
, '{jobid}'			-- pk_job 项目
, '{deptid}'			-- pk_deptdoc 部门 11076610000000009JDD
, 0									-- busilineno 业务单据行号
, NULL								-- exectime 转帐成功时间
, '{dateStr}'						-- tallydate 结算日期 Update
, NULL								-- pk_paybill 支付单ID
, 5									-- settlestatus 结算状态 Update NULL->5 已结算
, NULL								-- pk_rescenter 责任中心
, NULL								-- execdate 转帐成功日期
, '{bizhong}'			-- pk_currtype  币种
, NULL								-- pay_type 汇款速度
, NULL								-- signtime 签字时间
, '{pk_settlement}'			-- pk_settlement 结算信息主键
, '{fk_vouchid}'			-- pk_bill 付款单主键 来源单据id
, 'D3'								-- pk_billtype 来源单据类型 D3付款单
, 0									-- receivelocal 收款本币
, 0									-- payfrac 付款辅币
, 'MCNzb2Z0c2lnbra40mJOBdblHlI2sfA13+3a1ghspL3IHSE9wn4='		-- code 数字签名 Update
, {huilv}							-- localrate  本币汇率
, 'n,n,n,n,n,n,n,n,n,n,n,n,n'		-- modifyflag 手工修改标志
, NULL								-- pk_jobphase 项目阶段
, NULL								-- paybillcode 结算号
, 1									-- fundsflag  资金流向
, NULL, NULL, NULL					--  def20,bankmsg 银行返回信息,notenumber 票据号
, '{pk_settlement}.{i + 1}'			-- pk_detail 银行账主键  主键 11076610000000009NTY
, NULL, NULL						-- pk_invcl 存货分类，pk_plansubj 计划项目
, '{hbbm}'			-- pk_cubasdoc 供应商主键
, 0									-- notedirection 票据方向
, NULL								-- pk_assaccount 辅助帐号
, 1									-- direction 方向
, NULL, NULL						-- pk_bank 本方银行主键, groupno 分组号
, '{bankNumber}'			-- pk_oppaccount 对方帐户主键
, '{item.GoodsNCCode}'			-- pk_invbasdoc 存货档案
, '{dateStr}'						-- billdate  单据日期
, {item.TaxTotal:F2}								-- pay 付款原币
, 0									-- receivefrac 收款辅币 
, 0									-- receive 收款原币
, NULL								-- pk_costsubj 收支项目
, '{dwbm}'							-- pk_corp 单位公司
, '{fkDanJuHao}'				-- billcode 单据编号
, NULL, NULL, NULL, NULL			-- issamecity 同城标志, pk_billbalatype 协议结算方式, pk_mngaccount 管理账户, isbillrecord 是否单据记录
, '{GetDateTimeString()}'
);


");
            }
            */
            #endregion

        }





        private static string GetDateTimeString()
        {
            return CommonHelper.GetDateTimeString();
        }
    }
}
