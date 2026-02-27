export type DateOnly = string & { readonly __brand: 'dateonly' };

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
  lastScreeningDate: DateOnly | null;
  gender: string;
}

export const createPatient = (data: Partial<Patient> = {}): Patient => {
  return {
    id: data.id ?? 0,
    familyName: data.familyName ?? '',
    givenName: data.givenName ?? '',
    lastScreeningDate: data.lastScreeningDate ?? null,
    gender: data.gender ?? 'Unknown',
  };
};

export function asDateOnly(value: string | null | undefined): DateOnly | null {
  if (!value) return null;

  if (!/^\d{4}-\d{2}-\d{2}$/.test(value)) {
    throw new Error(`Invalid DateOnly format: ${value}. Expected YYYY-MM-DD.`);
  }
  return value as DateOnly;
}

export function mapGenderStringToEnum(gender: string): Gender {
  const entry = Object.entries(Gender).find(([key]) => key.toLowerCase() === gender.toLowerCase());
  return entry ? (entry[1] as Gender) : Gender.Unknown;
}
