export type AdministrativeGender = 'male' | 'female' | 'other' | 'unknown';

// TODO: update this to make remove the need for manual intervention (do some
// research)
export interface Patient {
  id: number;
  familyName: string;
  givenName: string;
  lastScreeningDate: Date;
  gender: AdministrativeGender;
}
