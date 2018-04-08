﻿/*
 * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：力软-前端开发组
 * 日 期：2017.04.18
 * 描 述：提交发起流程	
 */
var acceptClick;
var bootstrap = function ($, learun) {
    "use strict";
    var type = learun.frameTab.currentIframe().type;


    var page = {
        init: function () {
            if (type == 2)
            {
                $('#processName').parent().remove();
            }
        },
    };
    // 保存数据
    acceptClick = function (callBack) {
        if (!$('#form').lrValidform()) {
            return false;
        }
        var formData = $('#form').lrGetFormData();

        callBack(formData);
        return true;
    };
    page.init();
}