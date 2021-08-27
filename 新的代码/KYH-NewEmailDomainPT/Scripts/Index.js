var app = angular.module('myApp', []);  //创建模块
QJDNSerial = null; QJpageIndex = null;
PaiXu = "FindDate DESC, DomainName" //默认一个排序
app.controller('mycontroller', function ($scope) {
    //默认1—Intrude突如其来勾选框
    $("#CheckBox3").attr("checked", true);
    $scope.IndexList = function (pageIndex) {
        //多选框板块筛选
        var TopicAreaStr = "";
        var TopicArea = [];
        $("input[type='checkbox']:checked").each(function (i, e) {
            TopicArea.push(e.value);
        });
        TopicAreaStr = TopicArea.join(",");  //逗号分开
        if (TopicAreaStr == "") {
            alert("类型必须选中一种！");
        }
        $.ajax({
            url: "/NewEmailDomainPT/NewEmailDomainPT/IndexData",
            data: {
                "DomainName": $("#DomainName").val(),
                "FindDate": $("#Start_Date").val(),
                "EndTime": $("#End_Date").val(),
                "pageIndex": pageIndex,
                "orderby": PaiXu,
                "TopicArea": TopicAreaStr
            },
            type: "POST",
            dataType: "json",
            success: function (data) {
                var list = data.Data;
                var rowSortNumber = 0;
                $("#notfindlist").remove();   //empty-子节点清空，不会删除本身
                if (list.length == 0) {
                    //第一种;
                    $("#OAContent").append("<tr id='notfindlist'><td colspan='6' class='text-center' style='color:red;font-size:20px'>未找到任何记录</td></tr>");
                    $scope.List = null;
                    $scope.$apply();
                    $("#myPage").css({ "display": "none" }); //隐藏分页
                }
                else {
                    $("#myPage").css({ "display": "block" });
                    $.each(list, function (key, value) {
                        rowSortNumber++; //自增序号
                        var date = new Date(value.FindDate);
                        value.FindDate = date.Format("yyyy-MM-dd");  //日期格式化
                        value.rowSortNumber = (pageIndex - 1) * 20 + rowSortNumber;
                    })
                    $scope.List = list;
                    $scope.$apply();
                    //console.log(JSON.stringify(res.Data.IndexList));
                    $("#myPage").sPage({
                        page: pageIndex,//当前页码，必填
                        total: data.count,//数据总条数，必填
                        pageSize: 20,//每页显示多少条数据，默认10条
                        totalTxt: "每页20条，共" + data.count + "笔记录",//数据总条数文字描述，{total}为占位符，默认"共{total}条"
                        showTotal: true,//是否显示总条数，默认关闭：false
                        showSkip: true,//是否显示跳页，默认关闭：false
                        showPN: true,//是否显示上下翻页，默认开启：true
                        prevPage: "上一页",//上翻页文字描述，默认“上一页”
                        nextPage: "下一页",//下翻页文字描述，默认“下一页”
                        backFun: function (page) {
                            $scope.IndexList(page); //点击分页按钮回调函数，返回当前页码
                        }


                    });
                    QJpageIndex = pageIndex;
                }
            }
        });
    }
    //修改新到的电邮域名信息模态框
    $scope.OpenModal = function (DNSerial, DomainName, FindDate, SourceType, Remarks) {
        //给模态框赋值
        document.getElementById('MTDomainName').innerText = DomainName;
        document.getElementById('MTFindDate').innerText = FindDate;
        //下拉框
        $("#MTSourceType option[value='" + SourceType + "']").prop("selected", "selected");
        $('#MTRemarks').val(Remarks);
        QJDNSerial = DNSerial;
    }
    //修改新到的电邮域名信息方法
    $scope.Update = function () {
        var DomainName = document.getElementById('MTDomainName').innerText;
        var FindDate = document.getElementById('MTFindDate').innerText;
        //下拉框 
        var SourceType = $("#MTSourceType").find("option:selected").val();
        var Remarks = $('#MTRemarks').val();
        $.ajax({
            url: "/NewEmailDomainPT/NewEmailDomainPT/Update",
            data: { "DNSerial": QJDNSerial, "DomainName": DomainName, "FindDate": FindDate, "SourceType": SourceType, "Remarks": Remarks },
            type: "POST",
            dataType: "json",
            success: function (data) {
                if (data > 0) {
                    swal('保存成功!', '', 'success') //提示框
                    //关闭模态框
                    $('#myModal').modal('hide');
                    //刷新页面
                    $scope.IndexList(QJpageIndex);
                    $scope.$apply();
                }
                else {
                    swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                }
            }
        })
    }
    //排序
    $scope.CheckImg = function (id) {
        //获取src图片名字
        var src = document.getElementById(id).src;
        src = src.substring(src.lastIndexOf("/") + 1, src.lastIndexOf("."));
        if (src == "Default" || src == "Desc") {
            document.getElementById(id).src = "/NewEmailDomainPT/Scripts/图标/Esc.png";
            PaiXu = "DomainName";
            if (id == "FindDateImg") { //发生日期升序
                PaiXu = "FindDate";
                document.getElementById('DomainNameImg').src = "/NewEmailDomainPT/Scripts/图标/Default.png";
            }
            else {
                document.getElementById('FindDateImg').src = "/NewEmailDomainPT/Scripts/图标/Default.png";
            }
        }
        if (src == "Esc") {
            document.getElementById(id).src = "/NewEmailDomainPT/Scripts/图标/Desc.png";
            PaiXu = "DomainName Desc";
            if (id == "FindDateImg") { //发生日期降序
                PaiXu = "FindDate Desc";
                document.getElementById('DomainNameImg').src = "/NewEmailDomainPT/Scripts/图标/Default.png";
            }
            else {
                document.getElementById('FindDateImg').src = "/NewEmailDomainPT/Scripts/图标/Default.png";
            }
        }
        $scope.IndexList(1);
    }
    $scope.IndexList(1);
})
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
$(function () {
    //勾选框点击事件
    $("input[type='checkbox']").click(function () {
        var allChecked = true;
        $("input[type=checkbox]").each(function () {
            if ($(this).attr("id") != "CheckBox1" && !$(this).is(':checked')) {
                allChecked = false;
            }
        });
        if (allChecked) {
            $("#CheckBox1").attr("checked", true);
        }
        else {
            $("#CheckBox1").attr("checked", false);
        }
    })
})
function checkbox() {
    if ($("#CheckBox1").is(':checked')) {
        //勾【全部】就选中所有
        $("input[type='checkbox']").prop("checked", true);
    }
    else {
        $("input[type='checkbox']").prop("checked", false);
    }
}