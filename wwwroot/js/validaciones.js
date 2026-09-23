import { agregarClases, removerClases, getElementById } from "./frontUtils.js";

export function setValidInputStyle(id) {
  const input = getElementById(id);
  if (input !== null) {
    removerClases(input, "is-invalid");
    // agregarClases(input, "is-valid");
  }
}

export function setInvalidInputStyle(id) {
  const input = getElementById(id);
  if (input !== null) {
    // removerClases(input, "is-valid");
    agregarClases(input, "is-invalid");
  }
}

export function resetValidationInputStyle(id) {
  const input = getElementById(id);
  if (input !== null) {
    // removerClases(input, "is-valid");
    removerClases(input, "is-invalid");
  }
}

export function setValidationErrorMessage(tooltipId, message) {
  const tooltip = getElementById(tooltipId);
  if (tooltip !== null) tooltip.innerText = message;
}

export function resetValidationErrorMessage(tooltipId) {
  const tooltip = getElementById(tooltipId);
  if (tooltip !== null) tooltip.innerText = "";
}

/**
 * @param  {...string} campos 
 */
export function resetValidaciones(...campos) {
    campos.forEach(c => {
        resetValidationInputStyle(c);
        resetValidationErrorMessage(`error_${c}`);
    });
}

export function validarFormSelect(value) {
  if (value === "") {
    return { errorMessage: "El campo es requerido" };
  }
  return;
}

export function validarFecha(fecha) {
  if (!fecha) return false;

  let fechaSel = new Date(fecha);
  if (!fechaSel) return false;
  fechaSel = fechaSel.getTime();

  const fechaAct = new Date().getTime();
  if (fechaSel > fechaAct) return false;
  
  return true;
}

export function validarCUIT(cadena) {
  if (!cadena || cadena.trim().length == 0) 
    return;

  if (cadena.length > 13) 
    return { errorMessage: "El CUIT es muy largo" };
  
  const regExp = /^\d{2}\-\d{7,8}\-\d{1}$/;
  if (!regExp.test(cadena)) 
    return { errorMessage: "El formato CUIT es inválido" };

  return;
}

/**
 * @param {string} cadena 
 */
export function validarNombreApellido(cadena) {
  if (cadena === undefined || cadena === null) return { errorMessage: "El campo es requerido" };
  if (typeof cadena !== "string") return { errorMessage: "El campo de ser de texto" };
  if (cadena.trim() === "") return { errorMessage: "El campo es requerido" };
  if (cadena.length > 100) return { errorMessage: "El máximo de caracteres de de 100" };
  return;
}

/**
 * @param {string} cadena 
 * @returns {undefined | { errorMessage: string }} El mensaje de error si falla la validación
 */
export function validarTelefono(cadena) {
  if (cadena.trim().length == 0) return;
  if (cadena.length > 20) return { errorMessage: "El máximo de caracteres de de 20." };

  // const regExp = /^(\+54\s)?0?(\d{2,4})\s(15\s)?(\d{4}-?\d{4})|(\d{3}-?\d{4})|(\d{2}-?\d{4})$/;
  const regExp = /^\+?[0-9\s\-]{6,20}$/;
  if (!regExp.test(cadena)) return { errorMessage: "El teléfono tiene caractares inválidos" };
  return;
}

export function validarEmail(cadena) {
  if (cadena.length > 100) return { errorMessage: "El máximo de caracteres de de 100" };
  const regExp = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/; // la ultima parte {2,} podria ser {2,4}
  if (!regExp.test(cadena)) return { errorMessage: "El EMAIL ingresado NO es valido" };
  return;
}

export function validarDireccion(cadena) {
  if (!cadena || cadena.trim().length === 0) 
    return;

  if (cadena.length > 100) {
    return { errorMessage: "La dirección no puede superar los 100 caracteres" };
  }
  return;
}

export function validarPrecio(cadena, requrido = true) {
  if (cadena.length === 0) {
    if (!requrido) return;
    
    return { errorMessage: "El campo es obligatorio" };
  }
  let precioNum = parseFloat(cadena.replace(",", "."));
  if (isNaN(precioNum)) {
    return { errorMessage: "Debe ser un número" };
  }
  if (precioNum <= 0) {
    return { errorMessage: "Debe ser mayor que 0" };
  }
  return;
}

export function validarFechaDeInputDate(cadena) {
  if (!cadena) return { errorMessage: "Fecha incompleta o incorrecta" };
  return;
}

export function validarFechaInicioDelContrato(inico, fin, terminacion, esNuevoContrato) {
  const fechaIni = new Date(inico + "T00:00:00");
    
  const msFechaIni = fechaIni.getTime();
  const msFechaFIn = new Date(fin + "T00:00:00").getTime();

  if (msFechaIni > msFechaFIn) return { errorMessage: "F. inicio es mayor a la f. fin del contrato" };
  if (terminacion) {
    const fechaTerm = new Date(terminacion + "T00:00:00").getTime();
    if (msFechaIni > fechaTerm) return { errorMessage: "F. inicio es mayor a la f. terminación del contrato" };
  }

  return;
}

export function validarFechaFinDelContrato(inico, fin, terminacion) {
  const fechaIni = new Date(inico).getTime();
  const fechaFIn = new Date(fin).getTime();

  if (fechaFIn < fechaIni) return { errorMessage: "F. fin es menor a la f. incio del contrato" };
  if (terminacion) {
    const fechaTerm = new Date(terminacion).getTime();
    if (fechaFIn > fechaTerm) return { errorMessage: "F. fin es mayor a la f. terminación del contrato" };
  }

  return;
}

export function validarFechaTerminacionDelContrato(inico, fin, terminacion) {
  if (terminacion) {
    const fechaIni = new Date(inico).getTime();
    const fechaFin = new Date(fin).getTime();
    const fechaTerm = new Date(terminacion).getTime();

    if (fechaTerm < fechaIni) return { errorMessage: "¿puede un contrato cancelarse antes de que inicie?" };
    if (fechaTerm > fechaFin) return { errorMessage: "F. terminación es mayor a la f. fin del contrato" };
  }

  return;
}