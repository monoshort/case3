const bestelNummerElem = document.getElementById("bestelNummer");
const openstaandBedragElem = document.getElementById("openstaandBedrag");
const betaaldBedragElem = document.getElementById("betaaldBedrag");
const verschilElem = document.getElementById("verschil");

const errorContainerElem = document.getElementById("errorContainer");
const submitButton = document.getElementById("submitButton");

const debounce = (func, wait, immediate) => {
	let timeout;
	return function () {
		const context = this, args = arguments;
		const later = function () {
			timeout = null;
			if (!immediate) {
				func.apply(context, args);
			}
		};
		const callNow = immediate && !timeout;
		clearTimeout(timeout);
		timeout = setTimeout(later, wait);
		if (callNow) {
			func.apply(context, args);
		}
	};
};

const bestelnummerChange = debounce(async (eventData) => {
	const bestelNummer = eventData.target.value;
	const response = await fetch(`/Bestelling/GetOpenstaandBedrag/${bestelNummer}`)
		.then((response) => {
			if (response.status !== 404) {
				return response.json();
			}
		});
	if (response) {
		betaaldBedragElem.max = response.openstaandBedrag;
		openstaandBedragElem.value = response.openstaandBedrag;
	} else {
		betaaldBedragElem.max = 0;
		openstaandBedragElem.value = 0;
	}

}, 500);

const validateInputs = () => {
	errorContainerElem.innerHTML = "";
	submitButton.disabled = false;
	let errormessages = [];
	if (openstaandBedragElem.value - betaaldBedragElem.value < 0) {
		errormessages.push("Meer betalen dan de factuur is niet mogelijk.");
	}
	if (errormessages.length !== 0) {
		submitButton.disabled = true;
	}
	return errormessages;
};

const betaaldBedragChange = (eventData) => {
	const errormessages = validateInputs();
	if (errormessages.length === 0) {
		const openstaandBedrag = openstaandBedragElem.value;
		const betaaldBedrag = +eventData.target.value;
		verschilElem.innerHTML = openstaandBedrag - betaaldBedrag;
	} else {
		for (const errormessage of errormessages) {
			const li = document.createElement("li");
			li.appendChild(document.createTextNode(errormessage));
			errorContainerElem.appendChild(li);
		}
	}
};

bestelNummerElem.addEventListener("keyup", bestelnummerChange);
betaaldBedragElem.addEventListener("keyup", betaaldBedragChange);