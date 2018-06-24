Ext.QuickTips.init();

var isyj = 0;

var pageSize = 15;

var myStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
       'IsBangding', 'BangDingTime', 'UserDenno', 'QiShiZhan', 'DaoDaZhan', 'SuoShuGongSi', 'GpsDeviceID', 'YunDanRemark', 'Gps_lastinfo', 'YunDanDenno', 'UserID', 'Gps_lasttime', 'Gps_distance', 'Gps_duration', 'QiShiZhan_QX', 'DaoDaZhan_QX', 'SalePerson', 'Purchaser', 'PurchaserPerson', 'PurchaserTel', 'CarrierCompany', 'CarrierPerson', 'CarrierTel', 'SpecialLine', 'SpecialLinePerson', 'SpecialLinePersonTel', 'SpecialLineTel', 'DaoDaAddress', 'QiShiAddress', 'Expect_Hour', 'Expect_ArriveTime', 'Actual_ArriveTime', 'speed', 'direct', 'temp', 'oil', 'battery', 'totalDis', 'vhcofflinemin', 'parkingmin'
    ],
    onPageChange: function (sto, nPage, sorters) {
        DataBind(nPage);
    }
});

var yjStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
       'IsBangding', 'BangDingTime', 'UserDenno', 'QiShiZhan', 'DaoDaZhan', 'SuoShuGongSi', 'GpsDeviceID', 'YunDanRemark', 'Gps_lastinfo', 'YunDanDenno', 'UserID', 'Gps_lasttime', 'Gps_distance', 'Gps_duration', 'QiShiZhan_QX', 'DaoDaZhan_QX', 'SalePerson', 'Purchaser', 'PurchaserPerson', 'PurchaserTel', 'CarrierCompany', 'CarrierPerson', 'CarrierTel', 'SpecialLine', 'SpecialLinePerson', 'SpecialLinePersonTel', 'SpecialLineTel', 'DaoDaAddress', 'QiShiAddress', 'Expect_Hour', 'Expect_ArriveTime', 'Actual_ArriveTime', 'speed', 'direct', 'temp', 'oil', 'battery', 'totalDis', 'vhcofflinemin', 'parkingmin'
    ],
    onPageChange: function (sto, nPage, sorters) {
        DataBindYJ(nPage);
    }
});

var lsStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
       'IsBangding', 'BangDingTime', 'UserDenno', 'QiShiZhan', 'DaoDaZhan', 'SuoShuGongSi', 'GpsDeviceID', 'YunDanRemark', 'Gps_lastinfo', 'YunDanDenno', 'UserID', 'Gps_lasttime', 'Gps_distance', 'Gps_duration', 'QiShiZhan_QX', 'DaoDaZhan_QX', 'SalePerson', 'Purchaser', 'PurchaserPerson', 'PurchaserTel', 'CarrierCompany', 'CarrierPerson', 'CarrierTel', 'SpecialLine', 'SpecialLinePerson', 'SpecialLinePersonTel', 'SpecialLineTel', 'DaoDaAddress', 'QiShiAddress', 'Expect_Hour', 'Expect_ArriveTime', 'Actual_ArriveTime', 'speed', 'direct', 'temp', 'oil', 'battery', 'totalDis', 'vhcofflinemin', 'parkingmin'
    ],
    onPageChange: function (sto, nPage, sorters) {
        DataBindLS(nPage);
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
                        xtype: 'tabpanel',
                        activeTab: 0,
                        items: [
                            {
                                xtype: 'panel',
                                layout: {
                                    type: 'fit'
                                },
                                title: '在途运单',
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
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '查看轨迹',
                                                locked: true,
                                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                                    if (record.data.IsBangding == true)
                                                        return "<a href='javascript:void(0);' onClick='ShowGJ(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看轨迹</a>　<a href='javascript:void(0);' onClick='JCBD(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>解除绑定</a>　<a href='javascript:void(0);' onClick='showmx(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看明细</a>";
                                                    else
                                                        return "<a href='javascript:void(0);' onClick='ShowGJ(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看轨迹</a>　<a href='javascript:void(0);' onClick='showmx(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看明细</a>";
                                                }
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'UserDenno',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                locked: true,
                                                text: '建单号'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'SuoShuGongSi',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                locked: true,
                                                text: '建单公司'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'GpsDeviceID',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                locked: true,
                                                text: 'GPS设备号'
                                            },
                                            {
                                                xtype: 'datecolumn',
                                                dataIndex: 'BangDingTime',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                format: 'Y-m-d',
                                                text: '制单日期'
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
                                                dataIndex: 'PurchaserTel',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '收货方联系方式'
                                            },
                                            {
                                                xtype: 'datecolumn',
                                                dataIndex: 'Expect_ArriveTime',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                format: 'Y-m-d',
                                                text: '预计到达时间'
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
                                                dataIndex: 'Gps_distance',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '剩余路程'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'QiShiZhan',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '出发地',
                                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                                    if (record.data.QiShiZhan_QX != "" && record.data.QiShiZhan_QX != null)
                                                        return value + " " + record.data.QiShiZhan_QX;
                                                    else
                                                        return value;
                                                }
                                            },
                                            {
                                                xtype: 'datecolumn',
                                                dataIndex: 'Actual_ArriveTime',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                format: 'Y-m-d',
                                                text: '实际到达时间'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'DaoDaZhan',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '目的地',
                                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                                    if (record.data.DaoDaZhan_QX != "" && record.data.DaoDaZhan_QX != null)
                                                        return value + " " + record.data.DaoDaZhan_QX;
                                                    else
                                                        return value;
                                                }
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
                                                dataIndex: 'CarrierCompany',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运公司'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'CarrierTel',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运公司联系方式'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'SpecialLine',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运专线'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'SpecialLinePersonTel',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运专线联系方式'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'CarNumber',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运专线车辆车牌'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'CarPersonTel',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '驾驶员联系方式'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'QiShiAddress',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden:true,
                                                text: '出发详细地址'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'DaoDaAddress',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: true,
                                                text: '到达详细地址'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'Expect_Hour',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: true,
                                                text: '预计小时数'
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
                                                dataIndex: 'YunDanRemark',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '货物信息'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'PurchaserPerson',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: true,
                                                text: '收货人'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'CarrierPerson',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: true,
                                                text: '负责人'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'speed',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '速度(KM/h)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'direct',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '方向'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'temp',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '温度(℃)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'oil',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '油量(L)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'battery',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '电池(%)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'totalDis',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '总里程(Km)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'vhcofflinemin',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '不在线时长(min)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'parkingmin',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '停车超时时长(min)'
                                            }
                                        ],
                                        dockedItems: [
                                            {
                                                xtype: 'toolbar',
                                                dock: 'top',
                                                items: [
                                                    {
                                                        xtype: 'textfield',
                                                        width: 160,
                                                        labelWidth: 60,
                                                        id: 'SuoShuGongSi',
                                                        fieldLabel: '建单公司'
                                                    },
                                                    {
                                                        xtype: 'textfield',
                                                        width: 160,
                                                        labelWidth: 60,
                                                        id: 'UserDenno',
                                                        fieldLabel: '建单号'
                                                    },
                                                    {
                                                        xtype: 'textfield',
                                                        width: 150,
                                                        labelWidth: 70,
                                                        id: 'GpsDeviceID',
                                                        fieldLabel: 'GPS设备号'
                                                    },
                                                    {
                                                        xtype: 'datefield',
                                                        width: 160,
                                                        labelWidth: 60,
                                                        id: 'StartTime',
                                                        format: 'Y-m-d',
                                                        fieldLabel: '建单日期'
                                                    },
                                                    {
                                                        xtype: 'label',
                                                        text:'至'
                                                    },
                                                    {
                                                        xtype: 'datefield',
                                                        width: 100,
                                                        id: 'EndTime',
                                                        format: 'Y-m-d'
                                                    }
                                                ]
                                            },
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
                                                        width: 150,
                                                        labelWidth: 70,
                                                        id: 'Purchaser',
                                                        fieldLabel: '收货单位'
                                                    },
                                                    {
                                                        xtype: 'textfield',
                                                        width: 150,
                                                        labelWidth: 70,
                                                        id: 'CarrierCompany',
                                                        fieldLabel: '承运公司'
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
                                                        iconCls: 'download',
                                                        text: '下载',
                                                        handler: function () {
                                                            DownloadFile("CZCLZ.Handler.GetPoint_fileByZT", "在途下载.xls", Ext.getCmp('QiShiZhan_Province').getValue(), Ext.getCmp('QiShiZhan_City').getValue(), Ext.getCmp('QiShiZhan_Qx').getValue(), Ext.getCmp('DaoDaZhan_Province').getValue(), Ext.getCmp('DaoDaZhan_City').getValue(), Ext.getCmp('DaoDaZhan_Qx').getValue(), Ext.getCmp('SuoShuGongSi').getValue(), Ext.getCmp('GpsDeviceID').getValue(), Ext.getCmp('UserDenno').getValue(), Ext.getCmp('StartTime').getValue(), Ext.getCmp('EndTime').getValue(), Ext.getCmp('Purchaser').getValue(), Ext.getCmp('CarrierCompany').getValue());
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
                            },
                            {
                                xtype: 'panel',
                                title: '预警运单',
                                layout: {
                                    type: 'fit'
                                },
                                items: [
                                    {
                                        xtype: 'gridpanel',
                                        columnLines: 1,
                                        border: 1,
                                        store: yjStore,
                                        autoScroll: true,
                                        columns: [
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'UserID',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '查看轨迹',
                                                locked: true,
                                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                                    if (record.data.IsBangding == true)
                                                        return "<a href='javascript:void(0);' onClick='ShowGJ(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看轨迹</a>　<a href='javascript:void(0);' onClick='JCBD(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>解除绑定</a>　<a href='javascript:void(0);' onClick='showmx(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看明细</a>";
                                                    else
                                                        return "<a href='javascript:void(0);' onClick='ShowGJ(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看轨迹</a>　<a href='javascript:void(0);' onClick='showmx(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看明细</a>";
                                                }
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'UserDenno',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                locked: true,
                                                text: '建单号'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'SuoShuGongSi',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                locked: true,
                                                text: '建单公司'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'GpsDeviceID',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                locked: true,
                                                text: 'GPS设备号'
                                            },
                                            {
                                                xtype: 'datecolumn',
                                                dataIndex: 'BangDingTime',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                format: 'Y-m-d',
                                                text: '制单日期'
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
                                                dataIndex: 'PurchaserTel',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '收货方联系方式'
                                            },
                                            {
                                                xtype: 'datecolumn',
                                                dataIndex: 'Expect_ArriveTime',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                format: 'Y-m-d',
                                                text: '预计到达时间'
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
                                                dataIndex: 'Gps_distance',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '剩余路程'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'QiShiZhan',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '出发地',
                                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                                    if (record.data.QiShiZhan_QX != "" && record.data.QiShiZhan_QX != null)
                                                        return value + " " + record.data.QiShiZhan_QX;
                                                    else
                                                        return value;
                                                }
                                            },
                                            {
                                                xtype: 'datecolumn',
                                                dataIndex: 'Actual_ArriveTime',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                format: 'Y-m-d',
                                                text: '实际到达时间'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'DaoDaZhan',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '目的地',
                                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                                    if (record.data.DaoDaZhan_QX != "" && record.data.DaoDaZhan_QX != null)
                                                        return value + " " + record.data.DaoDaZhan_QX;
                                                    else
                                                        return value;
                                                }
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
                                                dataIndex: 'CarrierCompany',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运公司'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'CarrierTel',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运公司联系方式'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'SpecialLine',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运专线'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'SpecialLinePersonTel',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运专线联系方式'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'CarNumber',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运专线车辆车牌'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'CarPersonTel',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '驾驶员联系方式'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'QiShiAddress',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: true,
                                                text: '出发详细地址'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'DaoDaAddress',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: true,
                                                text: '到达详细地址'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'Expect_Hour',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: true,
                                                text: '预计小时数'
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
                                                dataIndex: 'YunDanRemark',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '货物信息'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'PurchaserPerson',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: true,
                                                text: '收货人'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'CarrierPerson',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: true,
                                                text: '负责人'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'speed',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '速度(Km/h)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'direct',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '方向'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'temp',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '温度(℃)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'oil',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '油量(L)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'battery',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '电池(%)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'totalDis',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '总里程(Km)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'vhcofflinemin',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '不在线时长(min)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'parkingmin',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '停车超时时长(min)'
                                            }
                                        ],
                                        dockedItems: [
                                            {
                                                xtype: 'toolbar',
                                                dock: 'top',
                                                items: [
                                                    {
                                                        xtype: 'textfield',
                                                        width: 160,
                                                        labelWidth: 60,
                                                        id: 'SuoShuGongSiYJ',
                                                        fieldLabel: '建单公司'
                                                    },
                                                    {
                                                        xtype: 'textfield',
                                                        width: 160,
                                                        labelWidth: 60,
                                                        id: 'UserDennoYJ',
                                                        fieldLabel: '建单号'
                                                    },
                                                    {
                                                        xtype: 'textfield',
                                                        width: 150,
                                                        labelWidth: 70,
                                                        id: 'GpsDeviceIDYJ',
                                                        fieldLabel: 'GPS设备号'
                                                    },
                                                    {
                                                        xtype: 'datefield',
                                                        width: 160,
                                                        labelWidth: 60,
                                                        id: 'StartTimeYJ',
                                                        format: 'Y-m-d',
                                                        fieldLabel: '建单日期'
                                                    },
                                                    {
                                                        xtype: 'label',
                                                        text: '至'
                                                    },
                                                    {
                                                        xtype: 'datefield',
                                                        width: 100,
                                                        id: 'EndTimeYJ',
                                                        format: 'Y-m-d'
                                                    }
                                                ]
                                            },
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
                                                        id: 'QiShiZhan_ProvinceYJ',
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
                                                        id: 'QiShiZhan_CityYJ',
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
                                                        id: 'QiShiZhan_QxYJ'
                                                    },
                                                    {
                                                        xtype: 'combobox',
                                                        labelWidth: 50,
                                                        width: 140,
                                                        valueField: 'ID',
                                                        displayField: 'MC',
                                                        queryMode: 'local',
                                                        store: province,
                                                        id: 'DaoDaZhan_ProvinceYJ',
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
                                                        id: 'DaoDaZhan_CityYJ',
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
                                                        id: 'DaoDaZhan_QxYJ'
                                                    },
                                                    {
                                                        xtype: 'textfield',
                                                        width: 150,
                                                        labelWidth: 70,
                                                        id: 'PurchaserYJ',
                                                        fieldLabel: '收货单位'
                                                    },
                                                    {
                                                        xtype: 'textfield',
                                                        width: 150,
                                                        labelWidth: 70,
                                                        id: 'CarrierCompanyYJ',
                                                        fieldLabel: '承运公司'
                                                    },
                                                    {
                                                        xtype: 'button',
                                                        iconCls: 'search',
                                                        text: '查询',
                                                        handler: function () {
                                                            isyj = 0;
                                                            DataBindYJ(1);
                                                        }
                                                    },
                                                    {
                                                        xtype: 'button',
                                                        iconCls: 'download',
                                                        text: '下载',
                                                        handler: function () {
                                                            DownloadFile("CZCLZ.Handler.GetPoint_fileByYJ", "预警下载.xls", Ext.getCmp('QiShiZhan_ProvinceYJ').getValue(), Ext.getCmp('QiShiZhan_CityYJ').getValue(), Ext.getCmp('QiShiZhan_QxYJ').getValue(), Ext.getCmp('DaoDaZhan_ProvinceYJ').getValue(), Ext.getCmp('DaoDaZhan_CityYJ').getValue(), Ext.getCmp('DaoDaZhan_QxYJ').getValue(), Ext.getCmp('SuoShuGongSiYJ').getValue(), Ext.getCmp('GpsDeviceIDYJ').getValue(), Ext.getCmp('UserDennoYJ').getValue(), Ext.getCmp('StartTimeYJ').getValue(), Ext.getCmp('EndTimeYJ').getValue(), Ext.getCmp('PurchaserYJ').getValue(), Ext.getCmp('CarrierCompanyYJ').getValue());
                                                        }
                                                    }
                                                ]
                                            },
                                            {
                                                xtype: 'pagingtoolbar',
                                                dock: 'bottom',
                                                width: 360,
                                                store: yjStore,
                                                displayInfo: true
                                            }
                                        ]
                                    }
                                ]
                            },
                            {
                                xtype: 'panel',
                                title: '历史运单',
                                layout: {
                                    type: 'fit'
                                },
                                items: [
                                    {
                                        xtype: 'gridpanel',
                                        columnLines: 1,
                                        border: 1,
                                        store: lsStore,
                                        columns: [
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'UserID',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '查看轨迹',
                                                locked: true,
                                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                                    if (record.data.IsBangding == true)
                                                        return "<a href='javascript:void(0);' onClick='ShowGJ(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看轨迹</a>　<a href='javascript:void(0);' onClick='JCBD(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>解除绑定</a>　<a href='javascript:void(0);' onClick='showmx(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看明细</a>";
                                                    else
                                                        return "<a href='javascript:void(0);' onClick='ShowGJ(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看轨迹</a>　<a href='javascript:void(0);' onClick='showmx(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看明细</a>";
                                                }
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'UserDenno',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                locked: true,
                                                text: '建单号'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'SuoShuGongSi',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                locked: true,
                                                text: '建单公司'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'GpsDeviceID',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                locked: true,
                                                text: 'GPS设备号'
                                            },
                                            {
                                                xtype: 'datecolumn',
                                                dataIndex: 'BangDingTime',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                format: 'Y-m-d',
                                                text: '制单日期'
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
                                                dataIndex: 'PurchaserTel',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '收货方联系方式'
                                            },
                                            {
                                                xtype: 'datecolumn',
                                                dataIndex: 'Expect_ArriveTime',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                format: 'Y-m-d',
                                                text: '预计到达时间'
                                            },
                                            {
                                                xtype: 'datecolumn',
                                                dataIndex: 'Actual_ArriveTime',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                format: 'Y-m-d',
                                                text: '实际到达时间'
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
                                                dataIndex: 'Gps_distance',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '剩余路程'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'QiShiZhan',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '出发地',
                                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                                    if (record.data.QiShiZhan_QX != "" && record.data.QiShiZhan_QX != null)
                                                        return value + " " + record.data.QiShiZhan_QX;
                                                    else
                                                        return value;
                                                }
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'DaoDaZhan',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '目的地',
                                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                                    if (record.data.DaoDaZhan_QX != "" && record.data.DaoDaZhan_QX != null)
                                                        return value + " " + record.data.DaoDaZhan_QX;
                                                    else
                                                        return value;
                                                }
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
                                                dataIndex: 'CarrierCompany',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运公司'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'CarrierTel',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运公司联系方式'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'SpecialLine',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运专线'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'SpecialLinePersonTel',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运专线联系方式'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'CarNumber',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '承运专线车辆车牌'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'CarPersonTel',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '驾驶员联系方式'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'QiShiAddress',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: true,
                                                text: '出发详细地址'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'DaoDaAddress',
                                                width: 200,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: true,
                                                text: '到达详细地址'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'Expect_Hour',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: true,
                                                text: '预计小时数'
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
                                                dataIndex: 'YunDanRemark',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: '货物信息'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'PurchaserPerson',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: true,
                                                text: '收货人'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'CarrierPerson',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: true,
                                                text: '负责人'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'speed',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '速度(Km/h)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'direct',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '方向'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'temp',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '温度(℃)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'oil',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '油量(L)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'battery',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '电池(%)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'totalDis',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '总里程(Km)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'vhcofflinemin',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '不在线时长(min)'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'parkingmin',
                                                width: 100,
                                                sortable: false,
                                                menuDisabled: true,
                                                hidden: false,
                                                text: '停车超时时长(min)'
                                            }
                                        ],
                                        dockedItems: [
                                            {
                                                xtype: 'toolbar',
                                                dock: 'top',
                                                items: [
                                                    {
                                                        xtype: 'textfield',
                                                        width: 160,
                                                        labelWidth: 60,
                                                        id: 'SuoShuGongSiLS',
                                                        fieldLabel: '建单公司'
                                                    },
                                                    {
                                                        xtype: 'textfield',
                                                        width: 160,
                                                        labelWidth: 60,
                                                        id: 'UserDennoLS',
                                                        fieldLabel: '建单号'
                                                    },
                                                    {
                                                        xtype: 'textfield',
                                                        width: 150,
                                                        labelWidth: 70,
                                                        id: 'GpsDeviceIDLS',
                                                        fieldLabel: 'GPS设备号'
                                                    },
                                                    {
                                                        xtype: 'datefield',
                                                        width: 160,
                                                        labelWidth: 60,
                                                        id: 'StartTimeLS',
                                                        format: 'Y-m-d',
                                                        fieldLabel: '建单日期'
                                                    },
                                                    {
                                                        xtype: 'label',
                                                        text: '至'
                                                    },
                                                    {
                                                        xtype: 'datefield',
                                                        width: 100,
                                                        id: 'EndTimeLS',
                                                        format: 'Y-m-d'
                                                    }
                                                ]
                                            },
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
                                                        id: 'QiShiZhan_ProvinceLS',
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
                                                        id: 'QiShiZhan_CityLS',
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
                                                        id: 'QiShiZhan_QxLS'
                                                    },
                                                    {
                                                        xtype: 'combobox',
                                                        labelWidth: 50,
                                                        width: 140,
                                                        valueField: 'ID',
                                                        displayField: 'MC',
                                                        queryMode: 'local',
                                                        store: province,
                                                        id: 'DaoDaZhan_ProvinceLS',
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
                                                        id: 'DaoDaZhan_CityLS',
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
                                                        id: 'DaoDaZhan_QxLS'
                                                    },
                                                    {
                                                        xtype: 'textfield',
                                                        width: 150,
                                                        labelWidth: 70,
                                                        id: 'PurchaserLS',
                                                        fieldLabel: '收货单位'
                                                    },
                                                    {
                                                        xtype: 'textfield',
                                                        width: 150,
                                                        labelWidth: 70,
                                                        id: 'CarrierCompanyLS',
                                                        fieldLabel: '承运公司'
                                                    },
                                                    {
                                                        xtype: 'button',
                                                        iconCls: 'search',
                                                        text: '查询',
                                                        handler: function () {
                                                            isyj = 0;
                                                            DataBindLS(1);
                                                        }
                                                    },
                                                    {
                                                        xtype: 'button',
                                                        iconCls: 'download',
                                                        text: '下载',
                                                        handler: function () {
                                                            DownloadFile("CZCLZ.Handler.GetPoint_fileByLS", "历史下载.xls", Ext.getCmp('QiShiZhan_ProvinceLS').getValue(), Ext.getCmp('QiShiZhan_CityLS').getValue(), Ext.getCmp('QiShiZhan_QxLS').getValue(), Ext.getCmp('DaoDaZhan_ProvinceLS').getValue(), Ext.getCmp('DaoDaZhan_CityLS').getValue(), Ext.getCmp('DaoDaZhan_QxLS').getValue(), Ext.getCmp('SuoShuGongSiLS').getValue(), Ext.getCmp('GpsDeviceIDLS').getValue(), Ext.getCmp('UserDennoLS').getValue(), Ext.getCmp('StartTimeLS').getValue(), Ext.getCmp('EndTimeLS').getValue(), Ext.getCmp('PurchaserLS').getValue(), Ext.getCmp('CarrierCompanyLS').getValue());
                                                        }
                                                    }
                                                ]
                                            },
                                            {
                                                xtype: 'pagingtoolbar',
                                                dock: 'bottom',
                                                width: 360,
                                                store: lsStore,
                                                displayInfo: true
                                            }
                                        ]
                                    }
                                ]
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

    DataBindYJ(1);

    DataBindLS(1);

    cityBind();
});

function showmx(UserID, YunDanDenno)
{
    CS('CZCLZ.Handler.SelectModelByUser', function (retVal) {
        if (retVal) {
            FrameStack.pushFrame({
                url: "approot/r/page/zhidan/EditYunDanNew.html?UserID=" + UserID + "&YunDanDenno=" + YunDanDenno,
                onClose: function (ret) {

                }
            });
        } else {
            FrameStack.pushFrame({
                url: "approot/r/page/zhidan/EditYunDan.html?UserID=" + UserID + "&YunDanDenno=" + YunDanDenno,
                onClose: function (ret) {

                }
            });
        }
    }, CS.onError);
}

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
    CS('CZCLZ.Handler.SearchMyYunDanByZT', function (retVal) {
        if (retVal) {
            myStore.setData({
                data: retVal.dt,
                pageSize: pageSize,
                total: retVal.ac,
                currentPage: retVal.cp
            });
        }
    }, CS.onError, cp, pageSize, Ext.getCmp('QiShiZhan_Province').getValue(), Ext.getCmp('QiShiZhan_City').getValue(), Ext.getCmp('QiShiZhan_Qx').getValue(), Ext.getCmp('DaoDaZhan_Province').getValue(), Ext.getCmp('DaoDaZhan_City').getValue(), Ext.getCmp('DaoDaZhan_Qx').getValue(), Ext.getCmp('SuoShuGongSi').getValue(), Ext.getCmp('GpsDeviceID').getValue(), Ext.getCmp('UserDenno').getValue(), Ext.getCmp('StartTime').getValue(), Ext.getCmp('EndTime').getValue(), Ext.getCmp('Purchaser').getValue(), Ext.getCmp('CarrierCompany').getValue());
}

function DataBindYJ(cp) {
    CS('CZCLZ.Handler.SearchMyYunDanByYJ', function (retVal) {
        if (retVal) {
            yjStore.setData({
                data: retVal.dt,
                pageSize: pageSize,
                total: retVal.ac,
                currentPage: retVal.cp
            });
        }
    }, CS.onError, cp, pageSize, Ext.getCmp('QiShiZhan_ProvinceYJ').getValue(), Ext.getCmp('QiShiZhan_CityYJ').getValue(), Ext.getCmp('QiShiZhan_QxYJ').getValue(), Ext.getCmp('DaoDaZhan_ProvinceYJ').getValue(), Ext.getCmp('DaoDaZhan_CityYJ').getValue(), Ext.getCmp('DaoDaZhan_QxYJ').getValue(), Ext.getCmp('SuoShuGongSiYJ').getValue(), Ext.getCmp('GpsDeviceIDYJ').getValue(), Ext.getCmp('UserDennoYJ').getValue(), Ext.getCmp('StartTimeYJ').getValue(), Ext.getCmp('EndTimeYJ').getValue(), Ext.getCmp('PurchaserYJ').getValue(), Ext.getCmp('CarrierCompanyYJ').getValue());
}

function DataBindLS(cp) {
    CS('CZCLZ.Handler.SearchMyYunDanByLS', function (retVal) {
        if (retVal) {
            lsStore.setData({
                data: retVal.dt,
                pageSize: pageSize,
                total: retVal.ac,
                currentPage: retVal.cp
            });
        }
    }, CS.onError, cp, pageSize, Ext.getCmp('QiShiZhan_ProvinceLS').getValue(), Ext.getCmp('QiShiZhan_CityLS').getValue(), Ext.getCmp('QiShiZhan_QxLS').getValue(), Ext.getCmp('DaoDaZhan_ProvinceLS').getValue(), Ext.getCmp('DaoDaZhan_CityLS').getValue(), Ext.getCmp('DaoDaZhan_QxLS').getValue(), Ext.getCmp('SuoShuGongSiLS').getValue(), Ext.getCmp('GpsDeviceIDLS').getValue(), Ext.getCmp('UserDennoLS').getValue(), Ext.getCmp('StartTimeLS').getValue(), Ext.getCmp('EndTimeLS').getValue(), Ext.getCmp('PurchaserLS').getValue(), Ext.getCmp('CarrierCompanyLS').getValue());
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
                        DataBindYJ(1);
                    });
                }
            }, CS.onError, UserID, YunDanDenno);
        }
    });
}
