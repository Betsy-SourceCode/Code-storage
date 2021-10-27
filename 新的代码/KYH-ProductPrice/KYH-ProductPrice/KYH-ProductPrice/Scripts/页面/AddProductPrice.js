$(function () {
    //账套下拉框
    var Ledger = "";
    $.ajax({
        type: "post",
        async: false,
        dataType: 'JSON',
        url: "/ProductPrice/ProductPrice/GetLedger",
        success: function (result) {
            $.each(result.Data, function (key, value) {
                Ledger += '<option value="' + value.Value + '"';
                Ledger += ">" + value.Text + '</option > ';
            })
            $("#Ledger").append(Ledger);
            //设置默认值
            $("#Ledger").find("option[value='YH']").attr("selected", true);
            return false;
        }
    });

    //货币下拉框
    var CurrList = "";
    $.ajax({
        type: "post",
        async: false,
        dataType: 'JSON',
        url: "/ProductPrice/ProductPrice/GetCurrList",
        success: function (result) {
            $.each(result.Data, function (key, value) {
                CurrList += '<option value="' + value.Value + '"';
                CurrList += ">" + value.Text + '</option > ';
            })
            $("#CurrList").append(CurrList);
            //设置默认值
            $("#CurrList").find("option[value='CNY']").attr("selected", true);
            return false;
        }
    });
})
//通过GIP产品号代入产品描述和客户产品代码
function CheckGipFnumber(GipFnumber, Num) {
    $('.example-popover').popover({ container: 'body', trigger: 'manual', html: 'true' });  //manual为手动模式用show控制显示
    var GipFnumber = $(GipFnumber).val();
    Num = $(Num).find("option:selected").val();  //账套下拉框
    $.ajax({
        type: "post",
        dataType: 'JSON',
        url: "/ProductPrice/ProductPrice/GetProductByGipFnumber",
        data: {
            "GipFnumber": GipFnumber,
            "Num": Num
        },
        success: function (result) {
            if (result.Success > 0) {
                //代入产品描述和客户产品代码
                $("#CustProdName").val(result.Data.CustProdName);  //产品描述
                $("#CustProdCode").val(result.Data.CustProdCode);  //客户产品代码
                $('.example-popover').popover('destroy'); //隐藏并销毁元素的弹出框
            }
            else {
                $('.example-popover').popover("show");
                //$('#Fnumber').val('');
                //document.getElementById('Fnumber').focus();
            }
        }
    })
}
function Onfocus() {
    $('.example-popover').popover('destroy'); //隐藏并销毁元素的弹出框
}
//保存数据
function SaveMyform() {
    var data = new FormData($('#myform')[0]);
    $.ajax({
        url: "/ProductPrice/ProductPriceZG/AddCustProductPrice?num=0",
        type: 'POST',
        dataType: 'json',
        data: data,
        contentType: false,// 告诉jQuery不要去设置Content-Type请求头
        processData: false,   // 告诉jQuery不要去处理发送的数据,使用FormData一定要加contentType和processData
        success: function (result) {
            if (result.Success > 0) {
                alert("添加成功！");
                window.location.href = "/ProductPrice/ProductPrice/DetailsProductPrice?CPSerial=" + result.id;//跳转到详情页
            }
            else {
                alert("添加失败，发生错误，请联系电脑部！内部成员请查看日志文件！");
            }
        }
    })
}
