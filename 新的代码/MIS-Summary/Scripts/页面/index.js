

layui.use(['table', 'layer', 'util'], function () {

    var table = layui.table,
        layer = layui.layer,
        util = layui.util;
    var time;//导出表格的时间
    var list;//所有数据
    var countList;//当前页的数据
    var result;
    //查询
    $("#btnSelect").click(function () {
        $('#btnExport').removeClass("layui-btn-disabled").prop("disabled", false);
        //DocumentNo, Mat_Code, Mat_Name, ApplyStartDate, ApplyEndDate
        var DocumentNo = $("#DocumentNo").val();//单据编号
        var Mat_Code = $("#Mat_Code").val();//物料代码
        var Mat_Name = $("#Mat_Name").val();//物料名称
        var ApplyStartDate = $("#ApplyStartDate").val();//验收单申请开始日期
        var ApplyEndDate = $("#ApplyEndDate").val();//验收单申请结束日期
        if (ApplyEndDate == "" && ApplyStartDate == "") { //判断验收单申请结束日期是否为空

        } else {
            if (ApplyStartDate != "" && ApplyEndDate == "") { //判断验收单申请开始日期是否为空
                layer.tips('必填项', '#ApplyEndDate', {
                    tips: [3, '#FF5722'],
                    time: 2000
                });
                return false;
            }
            if (ApplyEndDate != "" && ApplyStartDate == "") { //判断验收单申请结束日期是否为空
                layer.tips('必填项', '#ApplyStartDate', {
                    tips: [3, '#FF5722'],
                    time: 2000
                });
                return false;
            };
        };

        var DateTime = new Date();
        //导出表名时间转换
        var formatDateTime = function (date) {
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            m = m < 10 ? ('0' + m) : m;
            var d = date.getDate();
            d = d < 10 ? ('0' + d) : d;
            var h = date.getHours();
            var minute = date.getMinutes();
            minute = minute < 10 ? ('0' + minute) : minute;
            var Second = date.getSeconds();
            Second = Second < 10 ? ('0' + Second) : Second;
            return y + '' + m + '' + d + '' + h + '' + minute + '' + Second;
        };
        time = formatDateTime(DateTime);

        //显示的表格
        table.render({
            elem: '#SummaryTable'
            , url: '/Summary/Summary/IndexData'
            , where: { DocumentNo: DocumentNo, Mat_Code: Mat_Code, Mat_Name: Mat_Name, ApplyStartDate: ApplyStartDate, ApplyEndDate: ApplyEndDate }
            , title: "GIPACC" + time
            , height: 580
            , cols: [[
                { templet: '#xuhao', width: '3.8%', title: '序号<br/>Serial' }
                , {
                    field: 'ApplyDate', width: '5%', title: '验收单<br/>申请日期<br/>Apply <br/>Date'
                }
                , {
                    field: 'Serial_Num', width: '7.5%', title: '验收单编号<br/>Acceptance<br/> FormNumber', style: 'color:red;'
                }
                , { field: 'OA_number', width: '6.5%', title: '采购订单号<br/>PO Num' }
                , { field: 'PO', width: '8%', title: 'OA申请单号<br/>OA Flow Num' }
                , {
                    field: 'Fixed_number', width: '10%', title: '资产编号<br/>Asset Num', templet: function (d) {
                        var res;
                        if (d.Fixed_number.length > 30) { res = d.Fixed_number.substr(0, 30) + "..."; } else { res = d.Fixed_number } return res;
                    }
                }
                , { field: 'Mat_Code', width: '5.1%', title: '物料代码<br/>Material<br/>Code' }
                , { field: 'Mat_Name', width: '4.5%', title: '物料名称<br/>Material<br/> Name' }
                , {
                    field: 'Brand', width: '24%', title: '型号<br/>Model', templet: function (d) {
                        var res;
                        if (d.Brand.length > 90) { res = d.Brand.substr(0, 90) + "..."; } else { res = d.Brand } return res;
                    }
                }
                , { field: 'Unit', width: '3%', title: '单位<br/>Unit' }
                , { field: 'Qty', width: '3%', title: '数量<br/>Qty' }
                , { field: 'Mpn', width: '5.8%', title: '生产厂家<br/>Manufacturer' }
                , { field: 'Results', width: '4.5%', title: '验收结果<br/>Check<br/>Result' }
                , { field: 'belongCompany', width: '4.5%', title: '归属工厂<br/>Factory<br/>Belongs' }
                , { field: 'UseDept', width: '5.2%', title: '使用部门<br/>Operating<br/> Department' }
            ]]
            , page: true
            , limits: [15, 30, 40, 60]
            , limit: 15
            , parseData: function (res, curr) { //将原始数据解析成 table 组件所规定的数据
                list = res.data;//所有的数据

                if (this.page.curr) {
                    result = res.data.slice(this.limit * (this.page.curr - 1), this.limit * this.page.curr);
                } else {
                    result = res.data.slice(0, this.limit);
                }
                if (this.page.curr) {
                    list = res.data.slice(this.limit * (this.page.curr - 1), this.limit * this.page.curr);
                } else {
                    list = res.data.slice(0, this.limit);
                }


                return {
                    "code": res.code, //解析接口状态
                    "msg": res.msg, //解析提示文本
                    "count": res.count, //解析数据长度
                    "data": result //解析数据列表
                };
            },
            done: function (res, curr, count) {
                console.log(list)
                $.each(list, function (index, elemt) {
                    var Brandtitle = list[index].Brand;
                    //console.log(title)
                    $('tr').eq(index + 1).children("td").eq(8).each(function (i, e) {
                        $(e).attr('title', Brandtitle.toString());
                    });
                    var Fixed_numbertitle = list[index].Fixed_number;
                    $('tr').eq(index + 1).children("td").eq(5).each(function (i, e) {
                        $(e).attr('title', Fixed_numbertitle.toString());
                    });
                });

                $('thead>tr').css({ 'background-color': 'rgb(39,140,240)', 'color': 'white' });
                //tabTitle();//悬停显示全部内容
                //console.log(res.data);
            }
        });


        //导出的表格
        table.render({
            elem: '#SummaryTable1'
            , url: '/Summary/Summary/IndexData'
            , where: { DocumentNo: DocumentNo, Mat_Code: Mat_Code, Mat_Name: Mat_Name, ApplyStartDate: ApplyStartDate, ApplyEndDate: ApplyEndDate }
            , height: 530,
            title: "GIPACC" + time
            , cols: [[
                { templet: '#xuhao', width: '4%', title: '序号<br/>Serial' }
                , {
                    field: 'ApplyDate', width: '7%', title: '验收单申请日期<br/>Apply Date'
                }
                , {
                    field: 'Serial_Num', width: '9%', title: '验收单编号<br/>Acceptance FormNumber', style: 'color:red;'
                }
                , { field: 'OA_number', width: '7%', title: '采购订单号<br/>PO Num' }
                , { field: 'PO', width: '10%', title: 'OA申请单号<br/>OA Flow Num' }
                , {
                    field: 'Fixed_number', width: '6%', title: '资产编号<br/>Asset Num'
                }
                , { field: 'Mat_Code', width: '6%', title: '物料代码<br/>MaterialCode' }
                , { field: 'Mat_Name', width: '8%', title: '物料名称<br/>Material Name' }
                , {
                    field: 'Brand', width: '15%', title: '型号<br/>Model'
                },
                , { field: 'Unit', width: '3%', title: '单位<br/>Unit' }
                , { field: 'Qty', width: '3%', title: '数量<br/>Qty' }
                , { field: 'Mpn', width: '8%', title: '生产厂家<br/>Manufacturer' }
                , { field: 'Results', width: '4%', title: '验收结果<br/>CheckResult' }
                , { field: 'belongCompany', width: '5%', title: '归属工厂<br/>FactoryBelongs' }
                , { field: 'UseDept', width: 102, title: '使用部门<br/>Operating Department' }
            ]]
            ,
            done: function (res, curr, count) {

                $('thead>tr').css({ 'background-color': 'rgb(39,140,240)', 'color': 'white' });
                //console.log(res.data);
            }
        });
        //监听行单击事件（双击事件为：rowDouble）
        table.on('rowDouble(test)', function (obj) {
            var data = obj.data;
            //console.log(data.Serial_Num)

            $.ajax({
                url: '/Summary/Summary/NoApproveData',
                data: { Serial_Num: data.Serial_Num },//传值到后台
                dataType: 'json',
                success: function (res) {
                    if (res[0].Dept_Check == "") {
                        layer.open({
                            title: "",
                            // 基本层类型：0（信息框，默认）1（页面层）2（iframe层，也就是解析content）3（加载层）4（tips层）
                            type: 1,
                            // 当type: 2时就是url
                            content: "<div style='color:red;font-size:18px;text-align:center;padding-top:8%'>该验收单还没有最终审批<br/>暂不能打印！</div>",
                            // 宽高：如果是100%就是满屏
                            area: ['350px', '100px'],
                            // 坐标：auto（默认坐标，即垂直水平居中），具体当文档：https://www.layui.com/doc/modules/layer.html#offset
                            offset: 'auto',

                            // 遮罩：默认：0.3透明度的黑色背景（'#000'）
                            shade: 0.5,
                            // 是否点击遮罩关闭：默认：false
                            shadeClose: false,
                            // 自动关闭所需毫秒：默认：0不会自动关闭
                            time: 2000
                        });
                        //layer.msg('该验收单还没有最终审批，暂不能打印！', {
                        //    icon: 2,
                        //    skin: 'layer-ext-demo',
                        //    time: 2000
                        //})
                    } else {
                        layer.open({
                            type: 2,
                            title: '',
                            shadeClose: true,
                            shade: false,
                            maxmin: true,
                            area: ['70%', '92%'],
                            content: 'http://192.166.7.241:6500/EquipmentAcceptance/OAToExcel.aspx?Serial_Num=' + data.Serial_Num,
                            success: function () {
                                $(':focus').blur();//解决弹出层开启后点Enter回车无限弹出处理
                            }
                        });
                    }

                }
                , error: function () {
                    layer.msg('异常，请刷新后重试！', {
                        icon: 3,
                        skin: 'layer-ext-demo',
                        time: 2000
                    })
                }
            });

            //layer.alert(JSON.stringify(data), {
            //    title: '当前行数据：'
            //});

            ////标注选中样式
            //obj.tr.addClass('layui-table-click').siblings().removeClass('layui-table-click');
        });
    });
    //悬停显示全部内容
    function tabTitle() {

        $('td').each(function (index, element) {
            $(element).attr('title', $(element).text());
        });
    };
    //导出
    $("#btnExport").click(function () {
        var ExportLoad = layer.msg('导出中，请稍候', {
            icon: 16,
            shade: 0.2
        });
        $("#ExportData").table2excel({
            // 不被导出的表格行的CSS class类
            exclude: ".noExl",
            // 导出的Excel文档的名称
            name: "Excel Document Name",
            // Excel文件的名称
            filename: "GIPACC" + time
        });
        //table.exportFile('AllList', list, 'xls');
        layer.close(ExportLoad);
    });
});