layui.use(['table', 'laypage'], function () {

    var table = layui.table;

    var list;
    $("#btnSelect").click(function () {
        //DocumentNo, Mat_Code, Mat_Name, ApplyStartDate, ApplyEndDate
        var DocumentNo = $("#DocumentNo").val();//单据编号
        var Mat_Code = $("#Mat_Code").val();//物料代码
        var Mat_Name = $("#Mat_Name").val();//物料名称
        var ApplyStartDate = $("#ApplyStartDate").val();//验收单申请开始日期
        var ApplyEndDate = $("#ApplyEndDate").val();//验收单申请结束日期

        //if (DocumentNo == "" || DocumentNo == null) { //判断单据编号是否为空
        //    layer.tips('必填项', '#DocumentNo', {
        //        tips: [3, '#FF5722'],
        //        time: 2000
        //    });
        //    return false;
        //}
        //if (Mat_Code == "" || Mat_Code == null) { //判断物料代码是否为空
        //    layer.tips('必填项', '#Mat_Code', {
        //        tips: [3, '#FF5722'],
        //        time: 2000
        //    });
        //    return false;
        //}
        //if (Mat_Name == "" || Mat_Name == null) { //判断物料名称是否为空
        //    layer.tips('必填项', '#Mat_Name', {
        //        tips: [3, '#FF5722'],
        //        time: 2000
        //    });
        //    return false;
        //}
        //if (ApplyStartDate == "" || ApplyStartDate == null) { //判断验收单申请开始日期是否为空
        //    layer.tips('必填项', '#ApplyStartDate', {
        //        tips: [3, '#FF5722'],
        //        time: 2000
        //    });
        //    return false;
        //}
        //if (ApplyEndDate == "" || ApplyEndDate == null) { //判断验收单申请结束日期是否为空
        //    layer.tips('必填项', '#ApplyStartDate', {
        //        tips: [3, '#FF5722'],
        //        time: 2000
        //    });
        //    return false;
        //}

        table.render({
            elem: '#SummaryTable'
            , url: '/Summary/Summary/IndexData'
            , where: { DocumentNo: DocumentNo, Mat_Code: Mat_Code, Mat_Name: Mat_Name, ApplyStartDate: ApplyStartDate, ApplyEndDate: ApplyEndDate }
            ,height:530
            , cols: [[
                { templet: '#xuhao', width: '4%', title: '序号<br/>Serial' }
                , { field: 'ApplyDate', width: '7%', title: '验收单申请日期<br/>Apply Date' }
                , { field: 'Serial_Num', width: '9%', title: '验收单编号 <br/>Acceptance Form<br/>Number', style: 'color:red;' }
                , { field: 'OA_number', width: '7%', title: '采购订单号<br/>PO Num' }
                , { field: 'PO', width: '10%', title: 'OA申请单号<br/>OA Flow Num' }
                , {
                    field: 'Fixed_number', width: '6%', title: '资产编号<br/>Asset Num'
                }
                , { field: 'Mat_Code', width: '6%', title: '物料代码<br/>Material <br/>Code' }
                , { field: 'Mat_Name', width: '8%', title: '物料名称<br/>Material Name' }
                , { field: 'Brand', width: '15%', title: '型号<br/>Model' }
                , { field: 'Unit', width: '3%', title: '单位<br/>Unit' }
                , { field: 'Qty', width: '3%', title: '数量<br/>Qty' }
                , { field: 'Mpn', width: '8%', title: '生产厂家<br/>Manufacturer' }
                , { field: 'Results', width: '4%', title: '验收结果<br/>Check <br/>Result' }
                , { field: 'belongCompany', width: '5%', title: '归属工厂<br/>Factory <br/>Belongs' }
                , { field: 'UseDept', width: 102, title: '使用部门<br/>Operating <br/>Department' }
            ]]
            , page: true
            , limits: [15, 30, 40, 60]
            , limit: 15
            , parseData: function (res, curr) { //将原始数据解析成 table 组件所规定的数据
                var result;
                if (this.page.curr) {
                    result = res.data.slice(this.limit * (this.page.curr - 1), this.limit * this.page.curr);
                } else {
                    result = res.data.slice(0, this.limit);
                }

                return {
                    "code": res.code, //解析接口状态
                    "msg": res.msg, //解析提示文本
                    "count": res.count, //解析数据长度
                    "data": result //解析数据列表
                };
            },
            done: function (res, curr, count) {
                $('thead>tr').css({ 'background-color': 'lightblue', 'color': 'black' });
                tabTitle();//悬停显示全部内容
                console.log(res.data);
            }
        });
        //监听行单击事件（双击事件为：rowDouble）
        table.on('rowDouble(test)', function (obj) {
            var data = obj.data;
            console.log(data.Serial_Num)
            
            layer.open({
                type: 2,
                title: '',
                shadeClose: true,
                shade: false,
                maxmin: true,
                area: ['90%', '92%'],
                content: 'http://192.166.7.241:6500/EquipmentAcceptance/OAToExcel.aspx?Serial_Num=' + data.Serial_Num,
                success: function () {
                    $(':focus').blur();//解决弹出层开启后点Enter回车无限弹出处理
                }
            });
            //layer.alert(JSON.stringify(data), {
            //    title: '当前行数据：'
            //});

            ////标注选中样式
            //obj.tr.addClass('layui-table-click').siblings().removeClass('layui-table-click');
        });
        //悬停显示全部内容
        function tabTitle() {

            $('td').each(function (index, element) {
                $(element).attr('title', $(element).text());
            });
        };
        
    })
});