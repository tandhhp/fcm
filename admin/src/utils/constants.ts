export enum FormType {
  Holtel,
  PrivateStay,
  Flight,
  Activity,
  AirportTransfer,
  Tour,
  Healthcare,
}

export enum LeadStatus {
  Pending,
  Approved,
  Checkin,
  LeadAccept,
  LeadReject,
  ReInvite,
}

export enum TopupType {
  Topup,
  Debt,
}

export enum Tier {
  STANDARD,
  ELITE,
  ROYAL,
  PREMIER,
}

export enum UserStatus {
  Working,
  Leave
}

export enum TransactionType {
  Default,
  Bonus,
  Loan
}

export const SETTING_NAME = {
  CHATGPT: 'ChatGPT'
};

export const GENDER_OPTIONS = [
  {
    label: 'Nam',
    value: false
  },
  {
    label: 'Nữ',
    value: true
  }
];

export const SOURCE_CONTRACT_OPTIONS = [
  {
    label: 'PRIVATE',
    value: 1
  },
  {
    label: 'OUTSIDE',
    value: 5
  },
  {
    label: 'TELE INHOUSE',
    value: 3
  },
  {
    label: 'TELE Đại lý',
    value: 4
  }
]