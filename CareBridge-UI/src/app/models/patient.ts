export type DateOnly = string & { readonly __brand: 'dateonly' };

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
