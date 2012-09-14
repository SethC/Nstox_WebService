use FrontOff
if exists (select * from sysobjects where id = object_id(N'[dbo].[stp_IMExtract]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[stp_IMExtract]
go
set quoted_identifier  off    set ansi_nulls  on 
go
-- *******************************************************************************
-- DR/CR      Who         When        Why
-- ---------  ----------  ----------  --------------------------------------------
-- CR #12170  MARINAG     10/16/2002  Added the "Receipt Description" field to the 
--                                    extracted day/week PLU sales as the last field.
-- Created    MARINAG     02/19/2002
-- *******************************************************************************
create procedure stp_IMExtract (
  @sDataType char(1) ,     -- see *
  @dtFrom smalldatetime ,  -- from date
  @dtTo smalldatetime ,    -- to date
  @sExportDir varchar(500) -- directory for exported files
) as

-- * @sDataType:
--   'D' = DAY_PLU_SALES
--   'W' = WEEK_PLU_SALES

declare @sSql varchar(5000)
declare @sDateFrom char(10)
declare @sDateTo char(10)
declare @sFileDate char(7)

-- check data type
if @sDataType <> 'D' and @sDataType <> 'W'
begin
  set @sSql = right('00' + cast(datepart(mm, getdate()) as varchar(2)), 2) + '/' + right('00' + cast(datepart(dd, getdate()) as varchar(2)), 2) + '/' + cast(datepart(yyyy, getdate()) as varchar(4)) + ' ' + right('00' + cast(datepart(hh, getdate()) as varchar(4)), 2) + ':' + right('00' + cast(datepart(mi, getdate()) as varchar(2)), 2) + ':' + right('00' + cast(datepart(ss, getdate()) as varchar(2)), 2)
  set @sSql = 'echo ' + @sSql + ' - Unknown data type "' + @sDataType + '">>C:\PCMaster\Log\SLExtrct.log'
  exec master..xp_cmdshell @sSql, no_output
  return
end

-- set date variables for file name
set @sFileDate = right('00' + cast(datepart(mm, @dtFrom) as varchar(2)), 2) + right('00' + cast(datepart(dd, @dtFrom) as varchar(2)), 2) + '.' + right(cast(datepart(yyyy, @dtFrom) as varchar(4)), 2)
set @sDateFrom = right('00' + cast(datepart(mm, @dtFrom) as varchar(2)), 2) + '/' + right('00' + cast(datepart(dd, @dtFrom) as varchar(2)), 2) + '/' + cast(datepart(yyyy, @dtFrom) as varchar(4))
set @sDateTo = right('00' + cast(datepart(mm, @dtTo) as varchar(2)), 2) + '/' + right('00' + cast(datepart(dd, @dtTo) as varchar(2)), 2) + '/' + cast(datepart(yyyy, @dtTo) as varchar(4))

-- set directory for exported files
if @sExportDir = ''
  set @sExportDir = 'C:\Program Files\POSWare\Office\Export\'

-- Day movements
if @sDataType = 'D'
begin
  if exists (select * from sysobjects where id = object_id(N'[dbo].[vwDY]') and OBJECTPROPERTY(id, N'IsTable') = 1)
    drop table vwDY
  create table vwDY (
    DT char(19),
    ITM_ID char(20),
    SLS_PRC char(30),
    UNT_QTY char(30),
    WGT_ITM_FG char(5),
    SLS_AMT char(30),
    SLS_QTY char(30),
    ASSGN_PROM_AMT char(30),
    ASSGN_PROM_QTY char(30),
    ACTL_PROM_AMT char(30),
    ACTL_PROM_QTY char(30),
    DISC_AMT char(30),
    DISC_QTY char(30),
    PRC_OVRD_AMT char(30),
    PRC_OVRD_QTY char(30),
    REDUCED_PROM_QTY char(30),
    ON_SALE_AMT char(30),
    ON_SALE_QTY char(30),
    ON_SALE_MARKDOWN_AMT char(30),
    ITM_SCANN_QTY char(30),
    ITM_KEYD_QTY char(30),
    RTN_AMT char(30),
    RTN_QTY char(30),
    CPN_AMT char(30),
    CPN_QTY char(30),
    COST_CASE_PRC char(30),
    UNIT_CASE char(30),
    RCPT_DESCR char(20))

  insert into vwDY
  select
    right('00' + cast(datepart(mm, DAY_PLU_SALES.DT) as varchar(2)), 2) + '/' + right('00' + cast(datepart(dd, DAY_PLU_SALES.DT) as varchar(2)), 2) + '/' + cast(datepart(yyyy, DAY_PLU_SALES.DT) as varchar(4)) + ' ' + right('00' + cast(datepart(hh, DAY_PLU_SALES.DT) as varchar(4)), 2) + ':' + right('00' + cast(datepart(mi, DAY_PLU_SALES.DT) as varchar(2)), 2) + ':' + right('00' + cast(datepart(ss, DAY_PLU_SALES.DT) as varchar(2)), 2) ,
    str(DAY_PLU_SALES.ITM_ID, 20) 'ITM_ID' ,
    convert(char(30), DAY_PLU_SALES.SLS_PRC) 'SLS_PRC' ,
    convert(char(30), DAY_PLU_SALES.UNT_QTY) 'UNT_QTY' ,
    convert(char(5), DAY_PLU_SALES.WGT_ITM_FG) 'WGT_ITM_FG' ,
    convert(char(30), DAY_PLU_SALES.SLS_AMT) 'SLS_AMT' ,
    convert(char(30), DAY_PLU_SALES.SLS_QTY) 'SLS_QTY' ,
    convert(char(30), DAY_PLU_SALES.ASSGN_PROM_AMT) 'ASSGN_PROM_AMT' ,
    convert(char(30), DAY_PLU_SALES.ASSGN_PROM_QTY) 'ASSGN_PROM_QTY' ,
    convert(char(30), DAY_PLU_SALES.ACTL_PROM_AMT) 'ACTL_PROM_AMT' ,
    convert(char(30), DAY_PLU_SALES.ACTL_PROM_QTY) 'ACTL_PROM_QTY' ,
    convert(char(30), DAY_PLU_SALES.DISC_AMT) 'DISC_AMT' ,
    convert(char(30), DAY_PLU_SALES.DISC_QTY) 'DISC_QTY' ,
    convert(char(30), DAY_PLU_SALES.PRC_OVRD_AMT) 'PRC_OVRD_AMT' ,
    convert(char(30), DAY_PLU_SALES.PRC_OVRD_QTY) 'PRC_OVRD_QTY' ,
    convert(char(30), DAY_PLU_SALES.REDUCED_PROM_QTY) 'REDUCED_PROM_QTY' ,
    convert(char(30), DAY_PLU_SALES.ON_SALE_AMT) 'ON_SALE_AMT' ,
    convert(char(30), DAY_PLU_SALES.ON_SALE_QTY) 'ON_SALE_QTY' ,
    convert(char(30), DAY_PLU_SALES.ON_SALE_MARKDOWN_AMT) 'ON_SALE_MARKDOWN_AMT' ,
    convert(char(30), DAY_PLU_SALES.ITM_SCANN_QTY) 'ITM_SCANN_QTY' ,
    convert(char(30), DAY_PLU_SALES.ITM_KEYD_QTY) 'ITM_KEYD_QTY' ,
    convert(char(30), DAY_PLU_SALES.RTN_AMT) 'RTN_AMT' ,
    convert(char(30), DAY_PLU_SALES.RTN_QTY) 'RTN_QTY' ,
    convert(char(30), DAY_PLU_SALES.CPN_AMT) 'CPN_AMT' ,
    convert(char(30), DAY_PLU_SALES.CPN_QTY) 'CPN_QTY' ,
    convert(char(30), DAY_PLU_SALES.COST_CASE_PRC) 'COST_CASE_PRC' ,
    convert(char(30), DAY_PLU_SALES.UNIT_CASE) 'UNIT_CASE' ,
    isnull(PLU.RCPT_DESCR,'') 'RCPT_DESCR'
  from DAY_PLU_SALES, PLU
  where DAY_PLU_SALES.DT between @dtFrom and @dtTo
    and DAY_PLU_SALES.ITM_ID = PLU.ITM_ID

  set @sSql = 'bcp FrontOff..vwDY out "' + @sExportDir + 'DYMV' + @sFileDate + '" -T -c -t","'
  exec master..xp_cmdshell @sSql
  drop table vwDY

  set @sSql = right('00' + cast(datepart(mm, getdate()) as varchar(2)), 2) + '/' + right('00' + cast(datepart(dd, getdate()) as varchar(2)), 2) + '/' + cast(datepart(yyyy, getdate()) as varchar(4)) + ' ' + right('00' + cast(datepart(hh, getdate()) as varchar(4)), 2) + ':' + right('00' + cast(datepart(mi, getdate()) as varchar(2)), 2) + ':' + right('00' + cast(datepart(ss, getdate()) as varchar(2)), 2)
  set @sSql = 'echo ' + @sSql + ' - Exported records from DAY_PLU_SALES for dates ' + @sDateFrom + ' to ' + @sDateTo + '>>C:\PCMaster\Log\SLExtrct.log'
  exec master..xp_cmdshell @sSql, no_output
end else
-- Week movements
if @sDataType = 'W'
begin
  if exists (select * from sysobjects where id = object_id(N'[dbo].[vwWK]') and OBJECTPROPERTY(id, N'IsTable') = 1)
    drop table vwWK
  create table vwWK (
    DT char(19),
    ITM_ID char(20),
    SLS_PRC char(30),
    UNT_QTY char(30),
    WGT_ITM_FG char(5),
    SLS_AMT char(30),
    SLS_QTY char(30),
    ASSGN_PROM_AMT char(30),
    ASSGN_PROM_QTY char(30),
    ACTL_PROM_AMT char(30),
    ACTL_PROM_QTY char(30),
    DISC_AMT char(30),
    DISC_QTY char(30),
    PRC_OVRD_AMT char(30),
    PRC_OVRD_QTY char(30),
    REDUCED_PROM_QTY char(30),
    ON_SALE_AMT char(30),
    ON_SALE_QTY char(30),
    ON_SALE_MARKDOWN_AMT char(30),
    ITM_SCANN_QTY char(30),
    ITM_KEYD_QTY char(30),
    RTN_AMT char(30),
    RTN_QTY char(30),
    CPN_AMT char(30),
    CPN_QTY char(30),
    COST_CASE_PRC char(30),
    UNIT_CASE char(30),
    RCPT_DESCR char(20))

  insert into vwWK
  select
    right('00' + cast(datepart(mm, WEEK_PLU_SALES.DT) as varchar(2)), 2) + '/' + right('00' + cast(datepart(dd, WEEK_PLU_SALES.DT) as varchar(2)), 2) + '/' + cast(datepart(yyyy, WEEK_PLU_SALES.DT) as varchar(4)) + ' ' + right('00' + cast(datepart(hh, WEEK_PLU_SALES.DT) as varchar(4)), 2) + ':' + right('00' + cast(datepart(mi, WEEK_PLU_SALES.DT) as varchar(2)), 2) + ':' + right('00' + cast(datepart(ss, WEEK_PLU_SALES.DT) as varchar(2)), 2) ,
    str(WEEK_PLU_SALES.ITM_ID, 20) 'ITM_ID' ,
    convert(char(30), WEEK_PLU_SALES.SLS_PRC) 'SLS_PRC' ,
    convert(char(30), WEEK_PLU_SALES.UNT_QTY) 'UNT_QTY' ,
    convert(char(5), WEEK_PLU_SALES.WGT_ITM_FG) 'WGT_ITM_FG' ,
    convert(char(30), WEEK_PLU_SALES.SLS_AMT) 'SLS_AMT' ,
    convert(char(30), WEEK_PLU_SALES.SLS_QTY) 'SLS_QTY' ,
    convert(char(30), WEEK_PLU_SALES.ASSGN_PROM_AMT) 'ASSGN_PROM_AMT' ,
    convert(char(30), WEEK_PLU_SALES.ASSGN_PROM_QTY) 'ASSGN_PROM_QTY' ,
    convert(char(30), WEEK_PLU_SALES.ACTL_PROM_AMT) 'ACTL_PROM_AMT' ,
    convert(char(30), WEEK_PLU_SALES.ACTL_PROM_QTY) 'ACTL_PROM_QTY' ,
    convert(char(30), WEEK_PLU_SALES.DISC_AMT) 'DISC_AMT' ,
    convert(char(30), WEEK_PLU_SALES.DISC_QTY) 'DISC_QTY' ,
    convert(char(30), WEEK_PLU_SALES.PRC_OVRD_AMT) 'PRC_OVRD_AMT' ,
    convert(char(30), WEEK_PLU_SALES.PRC_OVRD_QTY) 'PRC_OVRD_QTY' ,
    convert(char(30), WEEK_PLU_SALES.REDUCED_PROM_QTY) 'REDUCED_PROM_QTY' ,
    convert(char(30), WEEK_PLU_SALES.ON_SALE_AMT) 'ON_SALE_AMT' ,
    convert(char(30), WEEK_PLU_SALES.ON_SALE_QTY) 'ON_SALE_QTY' ,
    convert(char(30), WEEK_PLU_SALES.ON_SALE_MARKDOWN_AMT) 'ON_SALE_MARKDOWN_AMT' ,
    convert(char(30), WEEK_PLU_SALES.ITM_SCANN_QTY) 'ITM_SCANN_QTY' ,
    convert(char(30), WEEK_PLU_SALES.ITM_KEYD_QTY) 'ITM_KEYD_QTY' ,
    convert(char(30), WEEK_PLU_SALES.RTN_AMT) 'RTN_AMT' ,
    convert(char(30), WEEK_PLU_SALES.RTN_QTY) 'RTN_QTY' ,
    convert(char(30), WEEK_PLU_SALES.CPN_AMT) 'CPN_AMT' ,
    convert(char(30), WEEK_PLU_SALES.CPN_QTY) 'CPN_QTY' ,
    convert(char(30), WEEK_PLU_SALES.COST_CASE_PRC) 'COST_CASE_PRC' ,
    convert(char(30), WEEK_PLU_SALES.UNIT_CASE) 'UNIT_CASE' ,
    isnull(PLU.RCPT_DESCR,'') 'RCPT_DESCR'
  from WEEK_PLU_SALES, PLU
  where WEEK_PLU_SALES.DT between @dtFrom and @dtTo
    and WEEK_PLU_SALES.ITM_ID = PLU.ITM_ID

  set @sSql = 'bcp FrontOff..vwWK out "' + @sExportDir + 'WKMV' + @sFileDate + '" -T -c -t","'
  exec master..xp_cmdshell @sSql
  drop table vwWK

  set @sSql = right('00' + cast(datepart(mm, getdate()) as varchar(2)), 2) + '/' + right('00' + cast(datepart(dd, getdate()) as varchar(2)), 2) + '/' + cast(datepart(yyyy, getdate()) as varchar(4)) + ' ' + right('00' + cast(datepart(hh, getdate()) as varchar(4)), 2) + ':' + right('00' + cast(datepart(mi, getdate()) as varchar(2)), 2) + ':' + right('00' + cast(datepart(ss, getdate()) as varchar(2)), 2)
  set @sSql = 'echo ' + @sSql + ' - Exported records from WEEK_PLU_SALES for dates ' + @sDateFrom + ' to ' + @sDateTo + '>>C:\PCMaster\Log\SLExtrct.log'
  exec master..xp_cmdshell @sSql, no_output
end
go
set quoted_identifier  off    set ansi_nulls  on 
go
