export type AdministrativeGender = 'male' | 'female' | 'other' | 'unknown';

export interface Patient {
  id: number;
  familyName: string;
  givenName: string;
  lastScreeningDate: Date;
  gender: AdministrativeGender;
}
