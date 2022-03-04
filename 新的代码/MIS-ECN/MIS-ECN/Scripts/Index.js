var m = new Map();   //m.get('Adam'); 键值对
$(function () {
    //日期控件绑定默认值 


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
        return y + '-' + m + '-' + d;
    };


    var date = new Date();

    year = date.getFullYear();
    month = date.getMonth();
    sdate = date.getDate();
    olddate = new Date(year, month - 6, sdate);
    time = formatDateTime(olddate);
    $("#Start_Date").val(time); //默认六个月之前
    /*    var myDate = new Date();*/
    $("#End_Date").val(date.Format("yyyy-MM-dd"));
    /* IndexList();*/
    //给下拉框赋值-生产工厂
    var ProduceFactory = "";
    $.ajax({
        type: "post",
        async: false,
        dataType: 'JSON',
        url: "/ECN/ECN/GetProducePlantDrop",
        success: function (result) {
            $.each(result, function (key, value) {
                ProduceFactory += '<option value="' + value + '"';
                ProduceFactory += ">" + value + '</option > ';
            })
            $("#ProduceFactory").append(ProduceFactory);
        }
    });
    //给下拉框赋值-主导工厂
    var LeadFactory = "";
    $.ajax({
        type: "post",
        async: false,
        dataType: 'JSON',
        url: "/ECN/ECN/GetDominFtyDrop",
        success: function (result) {
            $.each(result, function (key, value) {
                LeadFactory += '<option value="' + value + '"';
                LeadFactory += ">" + value + '</option > ';
            })
            $("#LeadFactory").append(LeadFactory);
        }
    });
})
var xh = 1;
function IndexList() {
    m.clear();
    $("tbody").html("");
    $("#count").html("");
    $("#myModal").modal({ backdrop: 'static', keyboard: false });
    $.ajax({
        url: "/ECN/ECN/IndexData",
        data: {
            "流程标题": $("#ProcessTitle").val(),
            "流程编号": $("#CERNumber").val(),
            "StartDate": $("#Start_Date").val(),
            "EndDate": $("#End_Date").val(),
            "产品型号": $("#ProductType").val(),
            "产品代码": $("#ProductCode").val(),
            "生产工厂": $("#ProduceFactory").val(),
            "主导工厂": $("#LeadFactory").val(),
            "流程状态": $("#Status").val()
        },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.length == 0) {
                //关闭导出按钮
                $('#btn_DaoChu').prop('disabled', true);
                $("#data").append("<tr id='notfindlist'><td colspan='28' class='text-left' style='color:red;font-size:20px;padding-left:893px'>未找到任何记录</td></tr>");
            }
            else {
                //打开导出按钮
                $('#btn_DaoChu').prop('disabled', false);
                var flownum = 0;     //审核流程次数
                $.each(data, function (key, value) {
                    if (m.has(value["流程编号"]) && value["流程状态"] != "5") {  //has某个键是否在当前 Map 对象
                        var obj = m.get(value["流程编号"]); //get方法读取key对应的键值，如果找不到key，返回undefined
                        var statusStr = "办理中";
                        if (value["流程状态"] == "0") {
                            statusStr = "办理中";
                        }
                        if (value["流程状态"] == "6") {
                            statusStr = "办理中";
                        }
                        else if (value["流程状态"] == "9") {
                            statusStr = "已办理完成";
                        }
                        else if (value["流程状态"] == "5") {
                            statusStr = "办理中";
                        }
                        obj.FlowStatus = statusStr;
                        //节点数组
                        var approvalStepNode = { "submitTime": "", "approveTime": "", "approvePerson": "", "timeConsumingWork": "", "timeConsuming": "", "stepNodeName": "" } //增加工作耗时 
                        approvalStepNode.submitTime = value["接收时间"];
                        approvalStepNode.approveTime = value["审批时间"];
                        approvalStepNode.approvePerson = value["审批人"];
                        approvalStepNode.timeConsumingWork = value["工作耗时"]; //工作耗时
                        var time = 0;
                        //计算工作耗时需要跳过周末，审批时间-接收时间
                        //if (approvalStepNode.approveTime.getDay() == 0 || approvalStepNode.approveTime.getDay() == 6) {
                        //    time += 480;
                        //}
                        var submitTime = new Date(approvalStepNode.submitTime);
                        var approveTime = new Date(approvalStepNode.approveTime);
                        var diffDay = (approveTime - submitTime) / (1000 * 60 * 60 * 24) + 1;
                        for (var f = 0; f < diffDay; f++) {
                            if (submitTime.getDay() == 0 || submitTime.getDay() == 6 || approveTime.getDay() == 0 || approveTime
                                .getDay() == 6) {
                                approvalStepNode.timeConsumingWork = parseInt(approvalStepNode.timeConsumingWork - 480); //减掉周末的8个小时
                            }
                            submitTime.setDate(submitTime.getDate() + 1);
                        }
                        approvalStepNode.timeConsumingWork = (((Math.floor(approvalStepNode.timeConsumingWork / 60)).toString().length) < 2 ? "0" + (Math.floor(approvalStepNode.timeConsumingWork / 60)).toString() : (Math.floor((approvalStepNode.timeConsumingWork / 60) * 100) / 100).toString());



                        approvalStepNode.timeConsuming = value["审批耗用时间"];
                        approvalStepNode.stepNodeName = value["审批节点"];
                        if (obj.ApprovalStepNodes[flownum] != undefined) {
                            obj.ApprovalStepNodes[flownum].push(approvalStepNode); //push()方法可向数组的末尾添加一个或多个元素，并返回新的长度
                        }
                        else {
                            //console.log(value["流程编号"]);
                        }
                    }
                    else {
                        if (value["流程状态"] == "5") { //当前流程编号存在退回数据
                            var obj = m.get(value["流程编号"]);
                            if (obj == undefined) {
                                var obj = {
                                    "Jia": "",
                                    "Number": "",
                                    "CERNumber": "",
                                    "Title": "",
                                    "ProductType": "",
                                    "ProductCode": "",
                                    "LeadFactory": "",
                                    "ProduceFactory": "",
                                    "ApprovelContent": "",
                                    "FlowStatus": "",
                                    "ApprovalStepNodes": [[]]
                                };
                                obj.Jia = "+";
                                obj.Number = "1";
                                obj.CERNumber = value["流程编号"];
                                obj.Title = value["流程标题"];
                                obj.ProductType = value["产品型号"];
                                obj.ProductCode = value["产品代码"];
                                obj.LeadFactory = value["主导工厂"];
                                obj.ProduceFactory = value["生产工厂"];
                                obj.ApprovelContent = value["申请更改内容"];
                                var statusStr = "办理中"; //默认为办理中
                                if (value["流程状态"] == "0") {
                                    statusStr = "办理中";
                                }
                                if (value["流程状态"] == "6") {
                                    statusStr = "办理中";
                                }
                                else if (value["流程状态"] == "9") {
                                    statusStr = "已办理完成";
                                }
                                else if (value["流程状态"] == "5") {
                                    statusStr = "办理中";
                                }
                                obj.FlowStatus = statusStr;
                                var approvalStepNode = { "submitTime": "", "approveTime": "", "approvePerson": "", "timeConsumingWork": "", "timeConsuming": "", "stepNodeName": "" } //增加工作耗时
                                approvalStepNode.submitTime = value["接收时间"];
                                approvalStepNode.approveTime = value["审批时间"];
                                approvalStepNode.approvePerson = value["审批人"];
                                approvalStepNode.timeConsumingWork = value["工作耗时"]; //工作耗时
                                approvalStepNode.timeConsuming = value["审批耗用时间"];
                                approvalStepNode.stepNodeName = value["审批节点"];
                                obj.ApprovalStepNodes[0].push(approvalStepNode);
                                m.set(value["流程编号"], obj);
                            }
                            if (value["审批节点"] == "ECR提出部门经理审批") {
                                var arr = new Array();
                                var approvalStepNode = { "submitTime": "", "approveTime": "", "approvePerson": "", "timeConsumingWork": "", "timeConsuming": "", "stepNodeName": "" } //增加工作耗时
                                approvalStepNode.submitTime = value["接收时间"];
                                approvalStepNode.approveTime = value["审批时间"];
                                approvalStepNode.approvePerson = value["审批人"];
                                approvalStepNode.timeConsumingWork = 0; //工作耗时
                                approvalStepNode.timeConsuming = value["审批耗用时间"];
                                approvalStepNode.stepNodeName = value["审批节点"];
                                var statusStr = "办理中"; //默认为办理中
                                if (value["流程状态"] == "0") {
                                    statusStr = "办理中";
                                }
                                if (value["流程状态"] == "6") {
                                    statusStr = "办理中";
                                }
                                else if (value["流程状态"] == "9") {
                                    statusStr = "已办理完成";
                                }
                                else if (value["流程状态"] == "5") {
                                    statusStr = "办理中";
                                }
                                obj.FlowStatus = statusStr;
                                //obj.ApprovalStepNodes[flownum].push(approvalStepNode);
                                if (obj.ApprovalStepNodes[flownum] != undefined) {
                                    obj.ApprovalStepNodes[flownum].push(approvalStepNode);
                                }
                                else {
                                    //console.log(value["流程编号"]);
                                }
                                obj.ApprovalStepNodes.push(arr);
                            }
                            else {
                                var approvalStepNode = { "submitTime": "", "approveTime": "", "approvePerson": "", "timeConsumingWork": "", "timeConsuming": "", "stepNodeName": "" } //增加工作耗时
                                approvalStepNode.submitTime = value["接收时间"];
                                approvalStepNode.approveTime = value["审批时间"];
                                approvalStepNode.approvePerson = value["审批人"];
                                approvalStepNode.timeConsumingWork = value["工作耗时"]; //工作耗时
                                approvalStepNode.timeConsuming = value["审批耗用时间"];
                                approvalStepNode.stepNodeName = value["审批节点"];
                                var statusStr = "办理中"; //默认为办理中
                                if (value["流程状态"] == "0") {
                                    statusStr = "办理中";
                                }
                                if (value["流程状态"] == "6") {
                                    statusStr = "办理中";
                                }
                                else if (value["流程状态"] == "9") {
                                    statusStr = "已办理完成";
                                }
                                else if (value["流程状态"] == "5") {
                                    statusStr = "办理中";
                                }
                                obj.FlowStatus = statusStr;
                                if (obj.ApprovalStepNodes[flownum] != undefined) {
                                    var arr = new Array();//JSON.parse(JSON.stringify(obj.ApprovalStepNodes[flownum]));//深拷贝
                                    obj.ApprovalStepNodes[flownum].push(approvalStepNode);
                                    obj.ApprovalStepNodes.push(arr);
                                }
                                else {
                                    //console.log(value["流程编号"]);
                                }
                            }
                            flownum = obj.ApprovalStepNodes.length - 1;
                        }
                        else {
                            flownum = 0;
                            var obj = {
                                "Jia": "",
                                "Number": "",
                                "CERNumber": "",
                                "Title": "",
                                "ProductType": "",
                                "ProductCode": "",
                                "LeadFactory": "",
                                "ProduceFactory": "",
                                "ApprovelContent": "",
                                "FlowStatus": "",
                                "ApprovalStepNodes": [[]]
                            };
                            obj.Jia = "+";
                            obj.Number = "1";
                            obj.CERNumber = value["流程编号"];
                            obj.Title = value["流程标题"];
                            obj.ProductType = value["产品型号"];
                            obj.ProductCode = value["产品代码"];
                            obj.LeadFactory = value["主导工厂"];
                            obj.ProduceFactory = value["生产工厂"];
                            obj.ApprovelContent = value["申请更改内容"];
                            var statusStr = "办理中"; //默认为办理中
                            if (value["流程状态"] == "0") {
                                statusStr = "办理中";
                            }
                            if (value["流程状态"] == "6") {
                                statusStr = "办理中";
                            }
                            else if (value["流程状态"] == "9") {
                                statusStr = "已办理完成";
                            }
                            else if (value["流程状态"] == "5") {
                                statusStr = "办理中";
                            }
                            obj.FlowStatus = statusStr;
                            var approvalStepNode = { "submitTime": "", "approveTime": "", "approvePerson": "", "timeConsumingWork": "", "timeConsuming": "", "stepNodeName": "" } //增加工作耗时
                            approvalStepNode.submitTime = value["接收时间"];
                            approvalStepNode.approveTime = value["审批时间"];
                            approvalStepNode.approvePerson = value["审批人"];
                            approvalStepNode.timeConsumingWork = value["工作耗时"]; //工作耗时
                            approvalStepNode.timeConsuming = value["审批耗用时间"];
                            approvalStepNode.stepNodeName = value["审批节点"];
                            obj.ApprovalStepNodes[0].push(approvalStepNode);
                            m.set(value["流程编号"], obj);
                        }
                    }
                });

                //$("#data").html("");
                m.forEach(function (value, key, index) {
                    //console.log(value);
                    var maxid = value.ApprovalStepNodes.length;
                    if (value.ApprovalStepNodes[maxid - 1].length == 0) {
                        value.ApprovalStepNodes.pop();
                    }
                    var TimeCount = 0;
                    if (value.FlowStatus == $("#Status option:selected").text() || $("#Status").val() == -1) {
                        for (var i = 0; i < value.ApprovalStepNodes.length; i++) {
                            for (var j = 0; j < value.ApprovalStepNodes[i].length; j++) {
                                TimeCount = TimeCount += value.ApprovalStepNodes[i][j].timeConsuming;
                            }
                        }
                        //console.log(TimeCount);
                        addtr(value, xh, TimeCount);
                        xh++;
                    };


                });
            }
            //页面渲染完之后再创建插件
            $('.example-popover').popover({ container: 'body', trigger: 'hover', html: 'true' });
            var tb = document.getElementById("data");
            if (tb.rows.length <= 0) {
                //关闭导出按钮
                $('#btn_DaoChu').prop('disabled', true);
                $("#data").append("<tr id='notfindlist'><td colspan='28' class='text-left' style='color:red;font-size:20px;padding-left:893px'>未找到任何记录</td></tr>");
            }
            //加载完成后  关闭遮罩层
            $("#myModal").modal('hide');
            $('.freeze-table').freezeTable({ 'scrollBar': true, 'columnNum': 11, 'freezeColumn': true });//冻结 
            $("#count").html(xh - 1);//显示总条数
            xh = 1;
        }
    });
}
function addtr(value, xh, TimeCount) {
    var tr = "";
    var stepName = ["申请人", "PM判断", "ECR提出部门经理审批", "RD文员", "RD工程师", "RD确认信息", "安规审批", "RD负责人审批", "CM_CP确认信息", "MC确认", "PC确认", "RD处理措施", "PMC审批", "EM审批", "ED审批", "QD审批", "MD审批", "RD经理审批", "RD指定人员打印"];
    var flowcount = value.ApprovalStepNodes.length;//审批流程次数

    for (var i = 0; i < flowcount; i++) {
        tr += '<tr onmouseover="myfun(this)" onmouseout="myfunremove(this)" onclick="ChangeBackColor(this)">'
        for (var j = 0; j < 12; j++) {
            if (j < 11) {
                switch (j) {
                    case 0:
                        tr += '<td onclick="Download(this)" class="noExl"> <p style="cursor: pointer"><span class="glyphicon glyphicon-plus noExl"></span ></p></td>' //加号预留位置
                        break
                    case 1:
                        tr += '<td>' + xh + '</td>' //自增序号
                        break
                    case 2:
                        tr += '<td  class="Redfont" ondblclick="Print(this)" style="cursor: pointer">' + value["CERNumber"] + '</td>' //ECR编号(打印请双击)
                        break
                    case 3: //流程标题
                        if (flowcount > 1) {
                            if (flowcount > 4) {
                                tr += '<td class="example-popover noExl"  data-content="<h5><b>' + value["Title"] + '<b/></h5>" data-placement="right"><div>' + value["Title"] + '</div></td>'
                            }
                            else {
                                tr += '<td class="example-popover noExl"  data-content="<h5><b>' + value["Title"] + '<b/></h5>" data-placement="right"><div class=" hiddens">' + value["Title"] + '</div></td> '
                            }

                        }
                        else {
                            if (value["Title"].length > 25) {
                                var Title = value["Title"].substring(0, 25) + "...";
                                tr += '<td class="example-popover noExl" data-content="<h5><b>' + value["Title"] + '<b/></h5>" data-placement="right">' + Title + '</td>'
                            }
                            else {
                                tr += '<td class="example-popover noExl" data-content="<h5><b>' + value["Title"] + '<b/></h5>" data-placement="right">' + value["Title"] + '</td>'
                            }
                        }
                        //导出专用
                        tr += '<td style="display:none">' + value["Title"] + '</td>'
                        break
                    case 4://产品型号
                        if (flowcount > 1) {
                            if (flowcount > 4) {
                                tr += '<td class="example-popover noExl hiddens"  data-content="<h5><b>' + value["ProductType"] + '<b/></h5>" data-placement="right"><div>' + value["ProductType"] + '</div></td>'
                            }
                            else {
                                tr += '<td class="example-popover noExl hiddens"  data-content="<h5><b>' + value["ProductType"] + '<b/></h5>" data-placement="right"><div class=" hiddens">' + value["ProductType"] + '</div></td>'
                            }

                        }
                        else {
                            if (value["ProductType"].length > 15) {
                                var ProductType = value["ProductType"].substring(0, 15) + "...";
                                tr += '<td class="example-popover noExl" data-content="<h5><b>' + value["ProductType"] + '<b/></h5>" data-placement="right">' + ProductType + '</td>'
                            }
                            else {
                                tr += '<td class="example-popover noExl" data-content="<h5><b>' + value["ProductType"] + '<b/></h5>" data-placement="right">' + value["ProductType"] + '</td>'
                            }
                        }


                        //导出专用
                        tr += '<td style="display:none">' + value["ProductType"] + '</td>'
                        break
                    case 5: //产品代码
                        if (flowcount > 1) {
                            if (flowcount > 4) {
                                tr += '<td class="example-popover noExl"  data-content="<h5><b>' + value["ProductCode"] + '<b/></h5>" data-placement="right"><div>' + value["ProductCode"] + '</div></td>'
                            }
                            else {
                                tr += '<td class="example-popover noExl hiddens"  data-content="<h5><b>' + value["ProductCode"] + '<b/></h5>" data-placement="right"><div class=" hiddens">' + value["ProductCode"] + '</div></td>'
                            }

                        }
                        else {
                            if (value["ProductCode"].length > 25) {
                                var ProductCode = value["ProductCode"].substring(0, 25) + "...";
                                tr += '<td class="example-popover noExl" data-content="<h5><b>' + value["ProductCode"] + '<b/></h5>" data-placement="right">' + ProductCode + '</td>'
                            }
                            else {
                                tr += '<td class="example-popover noExl" data-content="<h5><b>' + value["ProductCode"] + '<b/></h5>" data-placement="right">' + value["ProductCode"] + '</td>'
                            }
                        }


                        //导出专用
                        tr += '<td style="display:none">' + value["ProductCode"] + '</td>'
                        break
                    case 6:
                        tr += '<td>' + value["LeadFactory"] + '</td>'
                        break
                    case 7:
                        tr += '<td>' + value["ProduceFactory"] + '</td>'
                        break
                    case 8: //申请更改内容
                        if (flowcount > 1) {
                            if (flowcount > 4) {
                                tr += '<td class="example-popover noExl"  data-content="<h5><b>' + value["ApprovelContent"] + '<b/></h5>" data-placement="right"><div>' + value["ApprovelContent"] + '</div></td>'
                            }
                            else {
                                tr += '<td class="example-popover noExl hiddens"  data-content="<h5><b>' + value["ApprovelContent"] + '<b/></h5>" data-placement="right"><div class=" hiddens">' + value["ApprovelContent"] + '</div></td>'
                            }

                        }
                        else {
                            if (value["ApprovelContent"].length > 20) {
                                var ApprovelContent = value["ApprovelContent"].substring(0, 20) + "...";
                                tr += '<td class="example-popover noExl" data-content="<h5><b>' + value["ApprovelContent"] + '<b/></h5>" data-placement="right">' + ApprovelContent + '</td>'
                            }
                            else {
                                tr += '<td class="example-popover noExl" data-content="<h5><b>' + value["ApprovelContent"] + '<b/></h5>" data-placement="right">' + value["ApprovelContent"] + '</td>'
                            }
                        }


                        //导出专用
                        tr += '<td style="display:none">' + value["ApprovelContent"] + '</td>'
                        break
                    case 9:
                        tr += '<td>' + value["FlowStatus"] + '</td>'
                        break
                    case 10:
                        tr += '<td>' + TimeCount + '</td>'
                        break
                    default:
                        tr += '<td></td>'
                }
            }
            else {
                for (var step = 0; step < 19; step++) {
                    var hasValue = false;
                    for (var k = 0; k <= value["ApprovalStepNodes"][i].length; k++) {
                        if (value["ApprovalStepNodes"][i][k] == undefined) {   //步骤很少，没走完整流程,不做判断会报错
                            tr += '<td></td><td></td><td></td><td></td><td></td><td></td>';
                            break; //结束当前循环出去外循环
                        }
                        else {
                            if (stepName[step] == value["ApprovalStepNodes"][i][k]["stepNodeName"]) {  //遍历所有节点找出和当前数据节点对应的节点
                                //转换格式
                                var SubmitTimeDate = new Date(value["ApprovalStepNodes"][i][k]["submitTime"]);
                                tr += '<td>' + SubmitTimeDate.Format("yyyy-MM-dd hh:mm:ss ") + '&nbsp;</td>';
                                var ApproveTimeDate = new Date(value["ApprovalStepNodes"][i][k]["approveTime"]);
                                tr += '<td>' + ApproveTimeDate.Format("yyyy-MM-dd hh:mm:ss ") + '&nbsp;</td>';
                                tr += '<td>' + value["ApprovalStepNodes"][i][k]["approvePerson"] + '</td>';
                                tr += '<td>' + value["ApprovalStepNodes"][i][k]["timeConsumingWork"] + '</td>'; //工作耗时
                                tr += '<td>' + value["ApprovalStepNodes"][i][k]["timeConsuming"] + '</td>';
                                hasValue = true;//判断是否找到值
                                break;
                            }
                        }


                    }
                    //没找到值就默认添加四个空单元格
                    //if (!hasValue) {
                    //    tr += '<td></td><td></td><td></td><td></td>';
                    //}
                }
            }
        }
        tr += '</tr>'

    }
    $("#data").append(tr);
    var tb = document.getElementById("GridView");
    var endrow = tb.rows.length - 1;
    var startrow = endrow - flowcount;

    mergeCell("GridView", startrow, endrow, 14);
    mergeCell("GridView", flowcount, 8);
    mergeCell("GridView", flowcount, 7);
    mergeCell("GridView", flowcount, 6);
    mergeCell("GridView", flowcount, 5);
    mergeCell("GridView", flowcount, 4);
    mergeCell("GridView", flowcount, 3);
    mergeCell("GridView", flowcount, 2);
    mergeCell("GridView", flowcount, 1);
    mergeCell("GridView", flowcount, 0);
}
function myfun(obj) {
    $(obj).addClass("hoverClass");
    var rowspan = $(obj).find("td:first").attr('rowspan');
    // console.log(rowspan);
    if (rowspan != undefined) {
        var index = $('tr').index($(obj));
        tot = parseInt(index) + parseInt(rowspan);
        for (var i = index, len = tot; i < len; i++) {
            var trId = "table tr:eq(" + i + ")";
            $(trId).addClass("hoverClass");
        }
    }
}
function myfunremove(obj) {
    $(obj).removeClass("hoverClass");
    var rowspan = $(obj).find("td:first").attr('rowspan');
    if (rowspan != undefined) {
        var index = $('tr').index($(obj));
        tot = parseInt(index) + parseInt(rowspan);
        for (var i = index, len = tot; i < len; i++) {
            var trId = "table tr:eq(" + i + ")";
            $(trId).removeClass("hoverClass");
        }
    }
}
var flag = "";
var index = 0;
function ChangeBackColor(obj) {
    $(obj).siblings().removeClass("clickColor");
    $(obj).addClass("clickColor");
    var rowspan = $(obj).find("td:first").attr('rowspan');
    //console.log(rowspan);
    if (rowspan != undefined) {
        var index = $('tr').index($(obj));
        tot = parseInt(index) + parseInt(rowspan);
        for (var i = index, len = tot; i < len; i++) {
            var trId = "table tr:eq(" + i + ")";
            $(trId).addClass("clickColor");
        }
    }

}
//格式化时间
Date.prototype.Format = function (fmt) { // author: meizz
    var o = {
        "M+": this.getMonth() + 1, // 月份
        "d+": this.getDate(), // 日
        "h+": this.getHours(), // 小时
        "m+": this.getMinutes(), // 分
        "s+": this.getSeconds(), // 秒
        "q+": Math.floor((this.getMonth() + 3) / 3), // 季度
        "S": this.getMilliseconds() // 毫秒
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

/**
 * 合并单元格(如果结束行传0代表合并所有行)
 * @param table1    表格的ID
 * @param startRow  起始行
 * @param endRow    结束行
 * @param col   合并的列号，对第几列进行合并(从0开始)。第一行从0开始
 */
function mergeCell(table1, startRow, endRow, col) {
    var tb = document.getElementById(table1);
    if (!tb || !tb.rows || tb.rows.length <= 0) {
        return;
    }
    if (col >= tb.rows[0].cells.length || (startRow >= endRow && endRow != 0)) {
        return;
    }
    if (endRow == 0) {
        endRow = tb.rows.length - 1;
    }
    for (var i = startRow; i < endRow; i++) {
        //如果流程编号一样就合并0-8行
        if (tb.rows[i + 1] != undefined) {
            if (tb.rows[startRow].cells[2].innerHTML == tb.rows[i + 1].cells[2].innerHTML) { //如果相等就合并单元格,合并之后跳过下一行   
                //调整了一下，同时合并9列，减少循环次数
                for (var j = col; j >= 0; j--) {
                    tb.rows[i + 1].removeChild(tb.rows[i + 1].cells[j]);
                    tb.rows[startRow].cells[j].rowSpan = (tb.rows[startRow].cells[j].rowSpan) + 1;
                }
            } else {
                mergeCell(table1, i + 1, endRow, col);
                break;
            }
        }
        else {
            /* console.log("aaaa");*/
        }

    }
}
//二级表格显示文件下载路径
function Download(obj) {
    var MainId = $(obj).siblings().eq(1).html();
    var Id = $(obj).siblings().eq(0).html();
    var spanObj = $(obj).find("span:first");
    // $("#myModal").modal({ backdrop: 'static', keyboard: false });
    var tr = "";
    if (spanObj.hasClass("glyphicon-plus")) {    //如果是+改成-/如果是-改成+
        spanObj.removeClass("glyphicon-plus");
        spanObj.addClass("glyphicon-minus");

        $("#myModal").modal({ backdrop: 'static', keyboard: false });
        //通过Main获取路径
        $.ajax({
            url: "/ECN/ECN/ByMainGetDjbhName",
            data: { "CERNumber": MainId },
            type: "POST",
            dataType: "json",
            success: function (data) {

                if (data.length > 0) {
                    $.each(data, function (key, value) {
                        tr += '<tr class="' + MainId + '">'
                        tr += "<td colspan = '28' style='text-align:left;z-index:-1'><a href='#' onclick='GetHref(\"" + value["attachid"] + "\")' >" + value["filename"] + "</a></td >";
                        tr += '</tr >'
                    });
                }
                else {
                    tr += '<tr class="' + MainId + '">'
                    tr += "<td colspan = '28' style='text-align:left;z-index:-1'>暂无文档</td >";
                    tr += '</tr >'
                }
                //$(obj).parent().parent().parent().children().next().children().eq(3).after(tr);
                //获取当前数据合并多少行
                var index = "";
                var html = $(obj).siblings().eq(1).html();
                m.forEach(function (value) {
                    if (value["CERNumber"] == html) {
                        index = value.ApprovalStepNodes.length - 1;
                    }
                })
                if (index == 0) {
                    $(obj).parent().after(tr);
                } else {
                    $(obj).parent().nextAll().eq(index - 1).after(tr);
                }


                // console.log($(obj).parent().html());
            }, complete: function () {
                //加载完成后  关闭遮罩层
                $("#myModal").modal('hide');
            }
        });
    }
    else {
        spanObj.removeClass("glyphicon-minus");
        spanObj.addClass("glyphicon-plus");
        //移除二级表格的tr
        $("tbody").find("." + MainId).remove();
        //console.log($(obj).parent().parent().parent().children().next().children().eq(4).html());
    }
    //alert($(".clone-column-table-wrap").find("." + MainId).html());
    //alert($(obj).parent().html());
    //alert(Id);
    // $(".clone-column-table-wrap").find("#data").children().eq(0).after(tr);
    //alert($(".clone-column-table-wrap").find("#data").children().eq(0).html());
}

//下载文件
function GetHref(attachid) {
    var url = window.location.host;
    if (url == '192.166.7.241') {
        window.open('http://192.166.7.8/oaDownload/Default.aspx?id=' + attachid);
    }
    else {
        window.open('http://oa.gip4u.com/oaDownload/Default.aspx?id=' + attachid);
    }
}

//导出表格
function newApiArray() {
    var myDate = new Date();
    var time = myDate.Format("yyyyMMddhhmmss");  //获得当前年月日时分秒
    $("#GridView").table2excel({
        // 不被导出的表格行的CSS class类
        exclude: ".noExl",
        // 导出的Excel文档的名称
        name: "Excel Document Name",
        // Excel文件的名称
        filename: "ECN-" + time
    });
}
//打印跳转模板
function Print(obj) {
    var html = $(obj).html();
    window.open('http://192.166.7.241/ECNList/SECNV_3.aspx?FlowNo=' + html);
}


