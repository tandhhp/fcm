export default [
  {
    path: '/',
    redirect: '/home',
  },
  {
    path: '/accounts',
    layout: false,
    routes: [
      {
        name: 'login',
        path: '/accounts/login',
        component: './accounts/login'
      }
    ],
  },
  {
    icon: 'HomeOutlined',
    name: 'Trang chủ',
    path: '/home',
    component: './Home',
  },
  {
    name: 'Lịch làm việc',
    icon: 'CalendarOutlined',
    path: '/calendar',
    component: './calendar'
  },
  {
    name: 'component',
    path: '/works',
    hideInMenu: true,
    routes: [
      {
        name: 'block',
        path: '/works/:id',
        component: './works',
        hideInMenu: true,
      },
      {
        name: 'affiliateLink',
        path: '/works/affiliate-link/:id',
        component: './works/affiliate-link',
        hideInMenu: true,
      },
      {
        name: 'articleLister',
        path: '/works/article-lister/:id',
        component: './works/article-lister',
        hideInMenu: true,
      },
      {
        name: 'articlePicker',
        path: '/works/article-picker/:id',
        component: './works/article-picker',
        hideInMenu: true,
      },
      {
        name: 'articleSpotlight',
        path: '/works/article-spotlight/:id',
        component: './works/article-spotlight',
        hideInMenu: true,
      },
      {
        name: 'block',
        path: '/works/block/:id',
        component: './works/block',
        hideInMenu: true,
      },
      {
        name: 'editor',
        path: '/works/editor/:id',
        component: './works/editor',
        hideInMenu: true,
      },
      {
        name: 'facebookAlbum',
        path: '/works/facebook-album/:id',
        component: './works/facebook/album',
        hideInMenu: true,
      },
      {
        name: 'contactForm',
        path: '/works/contact-form/:id',
        component: './works/contact-form',
      },
      {
        name: 'feed',
        path: '/works/feed/:id',
        component: './works/feed',
      },
      {
        name: 'row',
        path: '/works/row/:id',
        component: './works/row',
        hideInMenu: true,
      },
      {
        name: 'image',
        path: '/works/image/:id',
        component: './works/image',
        hideInMenu: true,
      },
      {
        name: 'navbar',
        path: '/works/navbar/:id',
        component: './works/navbar',
        hideInMenu: true,
      },
      {
        name: 'swiper',
        path: '/works/swiper/:id',
        component: './works/swiper',
        hideInMenu: true,
      },
      {
        name: 'blockEditor',
        path: '/works/blockeditor/:id',
        component: './works/block-editor',
        hideInMenu: true,
      },
      {
        name: 'card',
        path: '/works/card/:id',
        component: './works/card',
        hideInMenu: true,
      },
      {
        name: 'googleMap',
        path: '/works/googlemap/:id',
        component: './works/google-map',
        hideInMenu: true,
      },
      {
        name: 'jumbotron',
        path: '/works/jumbotron/:id',
        component: './works/jumbotron',
        hideInMenu: true,
      },
      {
        name: 'masonry',
        path: '/works/masonry/:id',
        component: './works/masonry',
        hideInMenu: true,
      },
      {
        name: 'lookbook',
        path: '/works/lookbook/:id',
        component: './works/lookbook',
        hideInMenu: true,
      },
      {
        name: 'tag',
        path: '/works/tag/:id',
        component: './works/tag',
        hideInMenu: true,
      },
      {
        name: 'link',
        path: '/works/link/:id',
        component: './works/link',
        hideInMenu: true,
      },
      {
        name: 'listGroup',
        path: '/works/list-group/:id',
        component: './works/list-group',
        hideInMenu: true,
      },
      {
        name: 'productLister',
        path: '/works/product-lister/:id',
        component: './works/product-lister',
        hideInMenu: true,
      },
      {
        name: 'productPicker',
        path: '/works/product-picker/:id',
        component: './works/product-picker',
        hideInMenu: true,
      },
      {
        name: 'productSpotlight',
        path: '/works/product-spotlight/:id',
        component: './works/product-spotlight',
        hideInMenu: true,
      },
      {
        name: 'shopeeProduct',
        path: '/works/shopee-product/:id',
        component: './works/shopee-product',
        hideInMenu: true,
      },
      {
        name: 'sponsor',
        path: '/works/sponsor/:id',
        component: './works/sponsor',
        hideInMenu: true,
      },
      {
        name: 'trend',
        path: '/works/trend/:id',
        component: './works/trend',
        hideInMenu: true,
      },
      {
        name: 'videoPlayer',
        path: '/works/video-player/:id',
        component: './works/video-player',
        hideInMenu: true,
      },
      {
        name: 'videoPlaylist',
        path: '/works/video-playlist/:id',
        component: './works/video-playlist',
        hideInMenu: true,
      },
      {
        name: 'wordPressLister',
        path: '/works/wordpress-lister/:id',
        component: './works/wordpress-lister',
        hideInMenu: true,
      }
    ],
  },
  {
    name: 'Nội dung',
    path: '/catalog',
    icon: 'SlackOutlined',
    access: 'canCX',
    routes: [
      {
        name: 'Chăm sóc sức khỏe',
        path: '/catalog/product',
        component: './catalog/products'
      },
      {
        name: 'Cơ sở khám',
        path: '/catalog/hospital',
        component: './catalog/hospital'
      },
      {
        name: 'Gói khám',
        path: '/catalog/hospital/package/:id',
        component: './catalog/hospital/package',
        hideInMenu: true
      },
      {
        name: 'Dưỡng sinh độc bản',
        path: '/catalog/tour',
        component: './tour'
      },
      {
        name: 'Danh sách khách sạn',
        path: '/catalog/room',
        component: './tour/room'
      },
      {
        name: 'Tin tức',
        path: '/catalog/article',
        component: './catalog/article',
        hideInMenu: true
      },
      {
        name: 'Trang',
        path: '/catalog/page',
        component: './catalog/page'
      },
      {
        name: 'Bình luận',
        path: '/catalog/comments',
        component: './comments'
      },
      {
        name: 'Thành tựu',
        path: '/catalog/achievement',
        component: './achievement'
      },
      {
        name: 'Quà tặng',
        path: '/catalog/gift',
        component: './gift'
      }
    ]
  },
  {
    name: 'catalog',
    path: '/catalog/:id',
    component: './catalog',
    hideInMenu: true
  },
  {
    icon: 'ShoppingCartOutlined',
    name: 'Đơn đăng ký',
    path: '/form',
    component: './tour/forms',
    access: 'canForm'
  },
  {
    icon: 'IdcardOutlined',
    name: 'Liên hệ',
    path: '/contact',
    access: 'canTelesales',
    routes: [
      {
        name: 'Danh bạ',
        path: '/contact/index',
        component: './contact'
      },
      {
        name: 'Chi tiết liên hệ',
        path: '/contact/center/:id',
        component: './contact/center',
        hideInMenu: true
      },
      {
        name: 'Đã gọi',
        path: '/contact/dialed',
        component: './contact/dialed'
      },
      {
        name: 'Lịch hẹn',
        path: '/contact/confirm2',
        component: './contact/confirm2'
      },
      {
        name: 'Blacklist',
        path: '/contact/blacklist',
        component: './contact/blacklist'
      },
      {
        name: 'Nguồn dữ liệu',
        path: '/contact/call-source',
        component: './contact/source',
        access: 'can_read_page_contact_source'
      },
      {
        name: 'TMR Report',
        path: '/contact/call-status',
        component: './contact/call-status'
      },
      {
        name: 'Cấu hình nguồn',
        path: '/contact/source-setting',
        component: './settings/source',
        access: 'adminData'
      },
      {
        name: 'Tele Report',
        path: '/contact/report-tele',
        component: './contact/report/tele'
      },
      {
        name: 'CDR',
        path: '/contact/report/cdr',
        component: './contact/report/cdr'
      },
      {
        name: 'Sàn DOS',
        path: '/contact/dos',
        component: './contact/dos'
      },
      {
        name: 'Chi tiết DOS',
        path: '/contact/dos/tele/:id',
        component: './contact/dos/tele',
        hideInMenu: true
      }
    ]
  },
  {
    name: 'Hồ sơ',
    path: '/user/profile',
    component: './users/profile',
    hideInMenu: true,
  },
  {
    name: 'Bảo mật',
    path: '/user/center/:id',
    component: './users/center',
    hideInMenu: true,
  },
  {
    icon: 'BellOutlined',
    name: 'Thông báo',
    path: '/notification',
    component: './notification'
  },
  {
    icon: 'TeamOutlined',
    name: 'Người dùng',
    path: '/user',
    access: 'hr',
    routes: [
      {
        name: 'Chủ thẻ',
        path: '/user/member',
        component: './users/member',
        access: 'canCardHolder'
      },
      {
        name: 'Hoạt động',
        path: '/user/contact/activity/:id',
        component: './users/contact/activity',
        hideInMenu: true
      },
      {
        name: 'Nhân viên',
        path: '/user/roles',
        component: './users/roles',
        access: 'canHR',
      },
      {
        name: 'Chức vụ',
        path: '/user/roles/:id',
        component: './users/roles/center',
        hideInMenu: true
      },
      {
        name: 'Phòng - Ban',
        path: '/user/department',
        component: './department'
      },
      {
        name: 'Nhóm',
        path: '/user/department/team/:id',
        component: './department/team',
        hideInMenu: true
      },
      {
        name: 'Thành viên trong nhóm',
        path: '/user/department/team/user/:id',
        component: './users/team/user',
        hideInMenu: true
      }
    ],
  },
  {
    icon: 'UserSwitchOutlined',
    name: 'Hợp đồng',
    path: '/contract',
    component: './contract',
    access: 'canContract',
  },
  {
    icon: 'AccountBookOutlined',
    name: 'Tài chính',
    path: '/finance',
    access: 'can_read_page_finance',
    routes: [
      {
        name: 'Phiếu thu',
        path: '/finance/invoice',
        component: './finance/invoice'
      },
      {
        name: 'Phiếu chi',
        path: '/finance/bill',
        component: './finance/bill'
      },
      {
        name: 'Duyệt điểm',
        path: '/finance/loyalty',
        component: './users/loyalty'
      },
      {
        name: 'Báo cáo doanh số',
        path: '/finance/report',
        component: './debt'
      }
    ]
  },
  {
    icon: 'CalendarOutlined',
    name: 'Sự kiện',
    path: '/event',
    access: 'can_read_page_event',
    routes: [
      {
        name: 'Khung giờ',
        path: '/event/time-slot',
        component: './event',
      },
      {
        name: 'Danh sách tham dự',
        path: '/event/participate',
        component: './users/lead'
      },
      {
        name: 'Khung giờ trung tâm',
        path: '/event/time-slot/center/:id',
        component: './event/user',
        hideInMenu: true
      },
      {
        name: 'Check-In',
        path: '/event/checkin/:id',
        component: './event/user',
        hideInMenu: true
      },
      {
        name: 'Feedback',
        path: '/event/feedback',
        component: './event/feedback'
      },
      {
        name: 'Voucher',
        path: '/event/voucher',
        component: './event/campaign/voucher',
        access: 'event'
      },
      {
        name: 'Hợp đồng',
        path: '/event/contract',
        component: './event/contract',
        access: 'event'
      },
      {
        name: 'Phiếu thu',
        path: '/event/invoice',
        component: './event/invoice',
        access: 'can_read_page_event_invoice'
      },
      {
        name: 'Khách hàng',
        path: '/event/customer',
        component: './event/customer',
        access: 'can_read_page_event_customer'
      },
      {
        name: 'Show Up Report',
        path: '/event/report',
        component: './event/report',
        access: 'can_read_page_event_showup_report'
      }
    ]
  },
  {
    icon: 'SettingOutlined',
    name: 'Cài đặt',
    path: '/settings',
    access: 'canAdmin',
    routes: [
      {
        path: '/settings',
        redirect: '/settings/general',
      },
      {
        path: '/settings/general',
        name: 'Cài đặt chung',
        component: './settings'
      },
      {
        name: 'Thẻ',
        path: '/settings/card',
        component: './users/card',
        access: 'canAdmin',
      },
      {
        name: 'component',
        path: '/settings/component',
        component: './settings/components',
        hideInMenu: true,
      },
      {
        name: 'componentCenter',
        path: '/settings/component/center/:id',
        component: './settings/components/center',
        hideInMenu: true,
      },
      {
        name: 'google',
        path: '/settings/google/:id',
        component: './settings/google',
        hideInMenu: true,
      },
      {
        name: 'footer',
        path: '/settings/footer/:id',
        component: './settings/footer',
        hideInMenu: true,
      },
      {
        name: 'header',
        path: '/settings/header/:id',
        component: './settings/header',
        hideInMenu: true,
      },
      {
        name: 'style',
        path: '/settings/css',
        component: './settings/css',
        hideInMenu: true,
      },
      {
        name: 'telegram',
        path: '/settings/telegram/:id',
        component: './settings/telegram',
        hideInMenu: true,
      },
      {
        name: 'sendGrid',
        path: '/settings/sendgrid/:id',
        component: './settings/sendgrid',
        hideInMenu: true,
      },
      {
        name: 'facebook',
        path: '/settings/facebook/:id',
        component: './settings/facebook',
        hideInMenu: true,
      },
      {
        name: 'social',
        path: '/settings/social/:id',
        component: './settings/social',
        hideInMenu: true,
      },
      {
        name: 'Tỉnh/Thành phố',
        path: '/settings/province',
        component: './settings/province'
      },
      {
        name: 'Xã/Phường',
        path: '/settings/province/district/:id',
        component: './settings/province/district',
        hideInMenu: true,
      },
      {
        name: 'Nghề nghiệp',
        path: '/settings/job-kind',
        component: './settings/job-kind'
      },
      {
        name: 'Chi nhánh',
        path: '/settings/branch',
        component: './settings/branch'
      },
      {
        name: 'Phòng',
        path: '/settings/branch/room/:id',
        component: './settings/branch/room',
        hideInMenu: true
      },
      {
        name: 'Bàn',
        path: '/settings/branch/room/table/:id',
        component: './settings/branch/room/table',
        hideInMenu: true
      },
      {
        name: 'Trạng thái tham dự',
        path: '/settings/attendance',
        component: './settings/attendance'
      },
      {
        name: 'Nguồn',
        path: '/settings/source',
        component: './settings/source'
      }
    ],
  },
  {
    name: 'Người tham dự',
    path: '/event/user/:id',
    component: './event/user',
    access: 'canCX',
    hideInMenu: true
  },
  {
    name: 'Chat',
    component: './chat',
    path: '/chat',
    hideInMenu: true
  },
  {
    icon: 'InfoCircleOutlined',
    name: 'Lịch sử',
    path: '/history',
    access: 'canAdmin',
    component: './history'
  },
  {
    path: '*',
    layout: false,
    component: './404',
  }
]