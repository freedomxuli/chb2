Ext.QuickTips.init();

var pageSize = 15;

var myStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
        'BangDingTime', 'UserDenno', 'QiShiZhan', 'DaoDaZhan', 'SuoShuGongSi', 'GpsDeviceID', 'YunDanRemark', 'Gps_lastinfo', 'YunDanDenno', 'UserID', 'Gps_lasttime'
    ],
    onPageChange: function (sto, nPage, sorters) {
        DataBind(nPage);
    }
});

var newcity = {};

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

var city2 = Ext.create('Ext.data.Store', {
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
                        columns: [
                            {
                                xtype: 'datecolumn',
                                dataIndex: 'BangDingTime',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                format: 'Y-m-d',
                                text: '日期'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'UserDenno',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '运单号'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'QiShiZhan',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '起始站'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'DaoDaZhan',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '到达站'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'SuoShuGongSi',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '公司名称'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GpsDeviceID',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '设备ID'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'YunDanRemark',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '运单备注'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'Gps_lastinfo',
                                flex: 3,
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
                                dataIndex: 'YunDanDenno',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                hidden:true,
                                text: '随机运单号'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'UserID',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '查看轨迹',
                                renderer: function (value,cellmeta,record,rowIndex,columnIndex,store) {
                                    return "<a href='javascript:void(0);' onClick='ShowGJ(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看轨迹</a>";
                                }
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
                                        width: 100,
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: city,
                                        id: 'QiShiZhan_City'
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
                                        width: 100,
                                        valueField: 'ID',
                                        displayField: 'MC',
                                        queryMode: 'local',
                                        store: city2,
                                        id: 'DaoDaZhan_City'
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
                                        xtype: 'button',
                                        iconCls: 'search',
                                        text: '查询',
                                        handler: function () {
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
    for (var i = 0; i < cityList.provinces.length; i++) {
        var obj = {};
        obj.ID = cityList.provinces[i].name;
        obj.MC = cityList.provinces[i].name;
        provincesData.push(obj);

        var cityData = [];
        for (var j = 0; j < cityList.provinces[i].citys.length; j++) {
            var obj2 = {};
            obj2.ID = cityList.provinces[i].citys[j];
            obj2.MC = cityList.provinces[i].citys[j];
            cityData.push(obj2);
        }
        newcity[cityList.provinces[i].name] = cityData;
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
    }, CS.onError, cp, pageSize, Ext.getCmp('QiShiZhan_Province').getValue(), Ext.getCmp('QiShiZhan_City').getValue(), Ext.getCmp('DaoDaZhan_Province').getValue(), Ext.getCmp('DaoDaZhan_City').getValue(), Ext.getCmp('SuoShuGongSi').getValue(), Ext.getCmp('UserDenno').getValue());
}

function ShowGJ(UserID, YunDanDenno) {
    FrameStack.pushFrame({
        url: "chadanyundanguiji.html?UserID=" + UserID + "&YunDanDenno=" + YunDanDenno + "&type=wodeyundan",
        onClose: function (ret) {
            
        }
    });
}
