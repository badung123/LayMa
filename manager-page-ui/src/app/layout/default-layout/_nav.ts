import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
  //User
  {
    name: 'Thống kê',
    url: '/dashboard',
    iconComponent: { name: 'cil-speedometer' },
    attributes: {
      "policyName": "Permissions.Dashboard.View"
    }
  },
  {
    name: 'Link Rút Gọn',
    url: '/shortlink/list',
    iconComponent: { name: 'cil-cursor' },
    attributes: {
      "policyName": "Permissions.ShortLink.View"
    },
    children: [
      {
        name: 'Tạo link rút gọn',
        url: '/shortlink/create',
        icon: 'nav-icon-bullet',
        attributes: {
          "policyName": "Permissions.ShortLink.Create"
        }
      },
      {
        name: 'Quản lý link rút gọn',
        url: '/shortlink/list',
        icon: 'nav-icon-bullet',
        attributes: {
          "policyName": "Permissions.ShortLink.View"
        }
      }
    ]
  },
  {
    name: 'Đại lý',
    url: '/agent',
    iconComponent: { name: 'cil-people' },
    attributes: {
      "policyName": "Permissions.Agent.View"
    }
  },
  {
    name: 'Rút tiền',
    url: '/withdraw',
    iconComponent: { name: 'cil-puzzle' },
    attributes: {
      "policyName": "Permissions.WithDraw.Create"
    }
  },
  {
    name: 'Xác minh tài khoản',
    url: '/verifyAccount',
    iconComponent: { name: 'cil-check' },
    attributes: {
      "policyName": "Permissions.VerifyAccount.Create"
    }
  },
  //Admin
  {
    name: 'Chiến dịch',
    url: '/campain/google',
    iconComponent: { name: 'cil-cursor' },
    attributes: {
      "policyName": "Permissions.Campain.View"
    },
    children: [
      {
        name: 'Chiến dịch google',
        url: '/campain/google',
        icon: 'nav-icon-bullet',
        attributes: {
          "policyName": "Permissions.Campain.View"
        }
      },
      {
        name: 'Chiến dịch facebook',
        url: '/campain/facebook',
        icon: 'nav-icon-bullet',
        attributes: {
          "policyName": "Permissions.Campain.View"
        }
      }
    ]
  },
  {
    name: 'Quản lý rút tiền',
    url: '/withdrawmanager',
    iconComponent: { name: 'cil-puzzle' },
    attributes: {
      "policyName": "Permissions.AdminWithDraw.View"
    }
  },
  {
    name: 'Quản lý người dùng',
    url: '/user',
    iconComponent: { name: 'cil-puzzle' },
    attributes: {
      "policyName": "Permissions.Users.View"
    }
  },
  {
    name: 'Logs',
    url: '/logs',
    iconComponent: { name: 'cil-puzzle' },
    attributes: {
      "policyName": "Permissions.Log.View"
    }
  },
];
