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

export const CALL_STATUS_CODE = {
  TELE_NOT_UPDATE: 'TELE_NOT_UPDATE',
  NOT_AVAILABLE: 'NOT_AVAILABLE',
  NO_ANSWER: 'NO_ANSWER',
  WRONG_NUMBER: 'WRONG_NUMBER',
  POOR_HABIT: 'POOR_HABIT',
  POOR_FINANCIAL: 'POOR_FINANCIAL',
  CONFIRM1: 'CONFIRM1',
  CONSIDER: 'CONSIDER',
  ANOTHER_TIME: 'ANOTHER_TIME',
  CALL_LATER: 'CALL_LATER',
  CALL_UNDER_15S: 'CALL_UNDER_15S',
  NOT_INTERESTED: 'NOT_INTERESTED',
  LOCATION: 'LOCATION'
}