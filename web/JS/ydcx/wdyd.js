Ext.QuickTips.init();

var isyj = 0;

var pageSize = 15;

var myStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
       'IsBangding', 'BangDingTime', 'UserDenno', 'QiShiZhan', 'DaoDaZhan', 'SuoShuGongSi', 'GpsDeviceID', 'YunDanRemark', 'Gps_lastinfo', 'YunDanDenno', 'UserID', 'Gps_lasttime', 'Gps_distance', 'Gps_duration', 'QiShiZhan_QX', 'DaoDaZhan_QX', 'SalePerson', 'Purchaser', 'PurchaserPerson', 'PurchaserTel', 'CarrierCompany', 'CarrierPerson', 'CarrierTel', 'DaoDaAddress', 'QiShiAddress', 'Expect_Hour'
    ],
    onPageChange: function (sto, nPage, sorters) {
        DataBind(nPage);
    }
});

var bangStore = Ext.create('Ext.data.Store', {
    fields: [
        'ID', 'MC'
    ],
    data: [
        { 'ID': '', 'MC': '全部' },
        { 'ID': '0', 'MC': '历史运单' },
        { 'ID': '1', 'MC': '跟踪运单' }
    ]
});

var newcity = {};

var newqx = {};

var province = Ext.create('Ext.data.Store', {
    fields: [
        'ID', 'MC'
    ]
});

var city = Ext.create('Ext.data.Store', {
    fields: [
        'ID', 'MC'
    ]
});

var qx = Ext.create('Ext.data.Store', {
    fields: [
        'ID', 'MC'
    ]
});

var city2 = Ext.create('Ext.data.Store', {
    fields: [
        'ID', 'MC'
    ]
});

var qx2 = Ext.create('Ext.data.Store', {
    fields: [
        'ID', 'MC'
    ]
});

Ext.onReady(function () {

    Ext.define('mainView', {
        extend: 'Ext.container.Viewport',

        layout: {
            type: 'fit'
        },

        initComponent: function () {
            var me = this;

            Ext.applyIf(me, {
                items: [
                    {
                        xtype: 'gridpanel',
                        columnLines: 1,
                        border: 1,
                        store: myStore,
                        autoScroll: true,
                        columns: [
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'UserID',
                                width: 150,
                                sortable: false,
                                menuDisabled: true,
                                text: '查看轨迹',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                    if (record.data.IsBangding == true)
                                        return "<a href='javascript:void(0);' onClick='ShowGJ(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看轨迹</a>　<a href='javascript:void(0);' onClick='JCBD(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>解除绑定</a>";
                                    else
                                        return "<a href='javascript:void(0);' onClick='ShowGJ(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看轨迹</a>";
                                }
                            },
                            {
                                xtype: 'datecolumn',
                                dataIndex: 'BangDingTime',
                                width: 100,
                                sortable: false,
                                menuDisabled: true,
                                format: 'Y-m-d',
                                text: '日期'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'UserDenno',
                                width: 100,
                                sortable: false,
                                menuDisabled: true,
                                text: '运单号'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'QiShiZhan',
                                width: 200,
                                sortable: false,
                                menuDisabled: true,
                                text: '起始站',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                    if (record.data.QiShiZhan_QX != "" && record.data.QiShiZhan_QX != null)
                                        return value + " " + record.data.QiShiZhan_QX;
                                    else
                                        return value;
                                }
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'QiShiAddress',
                                width: 200,
                                sortable: false,
                                menuDisabled: true,
                                text: '出发详细地址'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'DaoDaZhan',
                                width: 200,
                                sortable: false,
                                menuDisabled: true,
                                text: '到达站',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                    if (record.data.DaoDaZhan_QX != "" && record.data.DaoDaZhan_QX != null)
                                        return value + " " + record.data.DaoDaZhan_QX;
                                    else
                                        return value;
                                }
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'DaoDaAddress',
                                width: 200,
                                sortable: false,
                                menuDisabled: true,
                                text: '到达详细地址'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'SuoShuGongSi',
                                width: 200,
                                sortable: false,
                                menuDisabled: true,
                                text: '公司名称'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GpsDeviceID',
                                width: 100,
                                sortable: false,
                                menuDisabled: true,
                                text: '设备ID'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'Expect_Hour',
                                width: 100,
                                sortable: false,
                                menuDisabled: true,
                                text: '预计小时数'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'Gps_distance',
                                width: 100,
                                sortable: false,
                                menuDisabled: true,
                                text: '剩余路程'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'Gps_duration',
                                width: 100,
                                sortable: false,
                                menuDisabled: true,
                                text: '剩余时间'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'YunDanRemark',
                                width: 100,
                                sortable: false,
                                menuDisabled: true,
                                text: '运单备注'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'Gps_lastinfo',
                                width: 700,
                                sortable: false,
                                menuDisabled: true,
                                text: '当前位置',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                    if (record.data.Gps_lasttime != "" && record.data.Gps_lasttime != null) {
                                        return "<span data-qtip='" + value + "'>" + "(" + record.data.Gps_lasttime.toCHString() + ")" + value + "</span>";
                                    }
                                    else {
                                        return "<span data-qtip='" + value + "'>" + value + "</span>";
                                        return value;
                                    }
                                }
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'SalePerson',
                                width: 100,
                                sortable: false,
                                menuDisabled: true,
                                text: '销售员'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'Purchaser',
                                width: 200,
                                sortable: false,
                                menuDisabled: true,
                                text: '收货单位'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'PurchaserPerson',
                                width: 100,
                                sortable: false,
                                menuDisabled: true,
                                text: '收货人'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'PurchaserTel',
                                width: 100,
                                sortable: false,
                                menuDisabled: true,
                                text: '联系电话'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'CarrierCompany',
                                width: 200,
                                sortable: false,
                                menuDisabled: true,
                                text: '承运公司'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'CarrierPerson',
                                width: 100,
                                sortable: false,
                                menuDisabled: true,
                                text: '负责人'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'CarrierTel',
                                width: 100,
                                sortable: false,
                                menuDisabled: true,
                                text: '联系电话'
                            }
                        ],
                        dockedItems: [
                            {
                                xtype: 'toolbar',
                                dock: 'top',
                                items: [
                                    {
                                        xtype: 'combobox',
                                        labelWidth: 50,
                                        width: 140,
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: province,
                                        id: 'QiShiZhan_Province',
                                        fieldLabel: '起始站',
                                        listeners: {
                                            change: function (data, newValue, oldValue, eOpts) {
                                                city.loadData(newcity[newValue]);
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'combobox',
                                        width: 90,
                                        padding: '0 10 10 0',
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: city,
                                        id: 'QiShiZhan_City',
                                        listeners: {
                                            change: function (data, newValue, oldValue, eOpts) {
                                                qx.loadData(newqx[newValue]);
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'combobox',
                                        width: 90,
                                        padding: '0 10 10 0',
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: qx,
                                        id: 'QiShiZhan_Qx'
                                    },
                                    {
                                        xtype: 'combobox',
                                        labelWidth: 50,
                                        width: 140,
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: province,
                                        id: 'DaoDaZhan_Province',
                                        fieldLabel: '到达站',
                                        listeners: {
                                            change: function (data, newValue, oldValue, eOpts) {
                                                city2.loadData(newcity[newValue]);
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'combobox',
                                        width: 90,
                                        padding: '0 10 10 0',
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: city2,
                                        id: 'DaoDaZhan_City',
                                        listeners: {
                                            change: function (data, newValue, oldValue, eOpts) {
                                                qx2.loadData(newqx[newValue]);
                                            }
                                        }

                                    },
                                    {
                                        xtype: 'combobox',
                                        width: 90,
                                        padding: '0 10 10 0',
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: qx2,
                                        id: 'DaoDaZhan_Qx'
                                    },
                                    {
                                        xtype: 'textfield',
                                        width: 130,
                                        labelWidth: 30,
                                        id: 'SuoShuGongSi',
                                        fieldLabel: '公司'
                                    },
                                    {
                                        xtype: 'textfield',
                                        width: 130,
                                        labelWidth: 30,
                                        id: 'UserDenno',
                                        fieldLabel: '单号'
                                    },
                                    {
                                        xtype: 'textfield',
                                        width: 130,
                                        labelWidth: 50,
                                        id: 'GpsDeviceID',
                                        fieldLabel: '设备号'
                                    },
                                    {
                                        xtype: 'combobox',
                                        width: 150,
                                        padding: '0 10 10 0',
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: bangStore,
                                        labelWidth: 60,
                                        id: 'IsBangding',
                                        fieldLabel: '查看类型',
                                        editable: false,
                                        value:''
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'search',
                                        text: '查询',
                                        handler: function () {
                                            isyj = 0;
                                            DataBind(1);
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'search',
                                        text: '预警查看',
                                        handler: function () {
                                            isyj = 1;
                                            DataBind(1);
                                        }
                                    }
                                ]
                            },
                            {
                                xtype: 'pagingtoolbar',
                                dock: 'bottom',
                                width: 360,
                                store: myStore,
                                displayInfo: true
                            }
                        ]
                    }
                ]
            });

            me.callParent(arguments);
        }

    });

    new mainView();

    DataBind(1);

    cityBind();
});

function cityBind() {
    var provincesData = [];
    for (var i = 0; i < cityData3.length; i++) {
        var obj = {};
        obj.ID = cityData3[i].text;
        obj.MC = cityData3[i].text;
        provincesData.push(obj);

        var cityData = [];
        for (var j = 0; j < cityData3[i].children.length; j++) {
            var obj2 = {};
            obj2.ID = cityData3[i].children[j].text;
            obj2.MC = cityData3[i].children[j].text;
            cityData.push(obj2);
            var qxData = [];
            for (var k = 0; k < cityData3[i].children[j].children.length; k++) {
                var obj3 = {};
                obj3.ID = cityData3[i].children[j].children[k].text;
                obj3.MC = cityData3[i].children[j].children[k].text;
                qxData.push(obj3);
            }

            newqx[cityData3[i].children[j].text] = qxData;
        }
        newcity[cityData3[i].text] = cityData;
    }
    province.loadData(provincesData);
}

function DataBind(cp) {
    CS('CZCLZ.Handler.SearchMyYunDan', function (retVal) {
        if (retVal) {
            myStore.setData({
                data: retVal.dt,
                pageSize: pageSize,
                total: retVal.ac,
                currentPage: retVal.cp
            });
        }
    }, CS.onError, cp, pageSize, Ext.getCmp('QiShiZhan_Province').getValue(), Ext.getCmp('QiShiZhan_City').getValue(), Ext.getCmp('QiShiZhan_Qx').getValue(), Ext.getCmp('DaoDaZhan_Province').getValue(), Ext.getCmp('DaoDaZhan_City').getValue(), Ext.getCmp('DaoDaZhan_Qx').getValue(), Ext.getCmp('SuoShuGongSi').getValue(), Ext.getCmp('GpsDeviceID').getValue(), Ext.getCmp('UserDenno').getValue(), Ext.getCmp("IsBangding").getValue(), isyj);
}

function ShowGJ(UserID, YunDanDenno) {
    FrameStack.pushFrame({
        url: "chadanyundanguiji.html?UserID=" + UserID + "&YunDanDenno=" + YunDanDenno + "&type=wodeyundan",
        onClose: function (ret) {
            
        }
    });
}

function JCBD(UserID, YunDanDenno)
{
    Ext.Msg.confirm("提示", "是否解绑该设备?", function (btn) {
        if (btn == "yes") {
            CS('CZCLZ.Handler.CloseBD', function (retVal) {
                if (retVal) {
                    Ext.Msg.alert("提示", "解除绑定成功！", function () {
                        DataBind(1);
                    });
                }
            }, CS.onError, UserID, YunDanDenno);
        }
    });
}
