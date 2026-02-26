export enum Gender {
  Unknown = 0,
  Male = 1,
  Female = 2,
  Other = 3,
}

export interface Patient {
  id: number;
  familyName: string;
  givenName: string;
  lastScreeningDate: string | Date;
  gender: Gender;
}

// the sentinel constant
export const DATE_MIN_VALUE = '0001-01-01T00:00:00';

export const createPatient = (data: Partial<Patient>): Patient => {
  return {
    id: data.id ?? 0,
    familyName: data.familyName ?? '',
    givenName: data.givenName ?? '',
    lastScreeningDate: data.lastScreeningDate ?? DATE_MIN_VALUE,
    gender: data.gender ?? Gender.Unknown,
  };
};
