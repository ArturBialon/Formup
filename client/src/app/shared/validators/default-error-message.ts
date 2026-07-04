import { ErrorMessage } from "../../_interfaces/error-message.interface";

export const DEFAULT_ERROR_MESSAGES: ErrorMessage = {
  required: 'COMMON.VALIDATION.REQUIRED',
  email: 'COMMON.VALIDATION.EMAIL_INVALID',
  minlength: 'COMMON.VALIDATION.TOO_SHORT',
  maxlength: 'COMMON.VALIDATION.TOO_LONG',
  max: 'COMMON.VALIDATION.TOO_HIGH'
}