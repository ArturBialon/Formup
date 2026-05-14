export interface ErrorMessage {
    required: string;
    email: string;
    minlength: string;
    maxlength: string;
    max:string;
    pattern?: string;
    custom?: string;
    mustMatch?: string,
    inputMask?: string,
    isLink?: string,
    isTrim?: string
  }