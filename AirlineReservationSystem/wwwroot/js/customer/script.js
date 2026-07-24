/* =========================================================
   PharaohWings — Main JavaScript (Full Enhanced Multi-Page)
   ========================================================= */

// ============ DATA ============
const CITIES = ['Cairo', 'Alexandria', 'Luxor', 'Aswan', 'Hurghada', 'Sharm El Sheikh', 'Giza', 'Dahab'];

const DISTANCES = {
  'Cairo-Luxor': 500, 'Cairo-Aswan': 680, 'Cairo-Hurghada': 450,
  'Cairo-Sharm El Sheikh': 380, 'Cairo-Alexandria': 220, 'Cairo-Giza': 20,
  'Alexandria-Luxor': 680, 'Alexandria-Aswan': 860,
  'Luxor-Aswan': 200, 'Luxor-Hurghada': 250,
  'Aswan-Hurghada': 380, 'Hurghada-Sharm El Sheikh': 200,
};

const POINTS_PER_100KM = 30;

const BAGGAGE_OPTIONS = [
  { id: 'none', label: 'Standard Allowance (Included)', price: 0 },
  { id: '10kg', label: '+10 kg Extra', price: 300 },
  { id: '20kg', label: '+20 kg Extra', price: 500 },
  { id: '30kg', label: '+30 kg Extra', price: 700 }
];

const DESTINATIONS = [
  { 
    name: 'Giza', tag: 'Land of Pyramids', img: 'https://images.unsplash.com/photo-1503177119275-0aa32b3a9368?w=800&q=80', desc: 'Home to the Great Pyramids and the Sphinx.', flight: 1200,
    activities: [
      { name: 'Great Pyramids Tour', icon: '🏛️', duration: '3-4 hrs', price: '300 EGP', image: 'https://images.unsplash.com/photo-1503177119275-0aa32b3a9368?w=600&q=80', description: 'Explore the Great Pyramid of Giza, the Pyramid of Khafre, and the Pyramid of Menkaure with an expert guide.' },
      { name: 'Camel Ride', icon: '🐪', duration: '1 hr', price: '250 EGP', image: 'https://images.unsplash.com/photo-1547234935-80c700199bb0?w=600&q=80', description: 'Enjoy a traditional camel ride around the pyramids with stunning photo opportunities.' },
      { name: 'Sound & Light Show', icon: '🌙', duration: '1.5 hrs', price: '400 EGP', image: 'https://images.unsplash.com/photo-1566127444979-b3d4b022c0dd?w=600&q=80', description: 'Experience the mesmerizing Sound and Light Show at the Pyramids of Giza.' }
    ]
  },
  { 
    name: 'Luxor', tag: 'Ancient Thebes', img: 'images/luxor.jpg', desc: 'The world\'s greatest open-air museum.', flight: 1200,
    activities: [
      { name: 'Karnak Temple Visit', icon: '🏛️', duration: '3-4 hrs', price: '200 EGP', image: 'https://images.unsplash.com/photo-1568322445389-f64ac2515020?w=600&q=80', description: 'Discover the magnificent Karnak Temple Complex, one of the largest religious buildings ever constructed.' },
      { name: 'Valley of the Kings', icon: '👑', duration: '2-3 hrs', price: '240 EGP', image: 'https://images.unsplash.com/photo-1572252009286-268acec5ca0a?w=600&q=80', description: 'Explore the ancient tombs of pharaohs including the famous tomb of Tutankhamun.' },
      { name: 'Hot Air Balloon Ride', icon: '🎈', duration: '1 hr', price: '1500 EGP', image: 'https://images.unsplash.com/photo-1507699622177-088516e71e25?w=600&q=80', description: 'Soar above Luxor at sunrise and witness the breathtaking views of the Nile and ancient temples.' }
    ]
  },
  { 
    name: 'Aswan', tag: 'Nile Jewel', img: 'images/Aswan.jpg', desc: 'Discover Philae Temple, Abu Simbel, and the majestic Nile.', flight: 1500,
    activities: [
      { name: 'Abu Simbel Temples', icon: '🏺', duration: '4-5 hrs', price: '600 EGP', image: 'https://images.unsplash.com/photo-1590152020426-a13641433900?w=600&q=80', description: 'Visit the magnificent temples of Ramses II and Nefertari, a UNESCO World Heritage Site.' },
      { name: 'Philae Temple', icon: '⛵', duration: '2 hrs', price: '180 EGP', image: 'https://images.unsplash.com/photo-1565552645632-d725f8bfc19a?w=600&q=80', description: 'Explore the beautiful island temple dedicated to the goddess Isis.' },
      { name: 'Nubian Village Tour', icon: '🏘️', duration: '3 hrs', price: '250 EGP', image: 'https://images.unsplash.com/photo-1587595431978-52311407c69b?w=600&q=80', description: 'Experience authentic Nubian culture, traditions, and hospitality in a colorful village.' }
    ]
  },
  { 
    name: 'Hurghada', tag: 'Red Sea Paradise', img: 'https://images.unsplash.com/photo-1544551763-46a013bb70d5?w=800&q=80', desc: 'Crystal-clear waters, vibrant coral reefs, and endless sunshine.', flight: 1100,
    activities: [
      { name: 'Scuba Diving', icon: '🤿', duration: '3-4 hrs', price: '800 EGP', image: 'https://images.unsplash.com/photo-1544551763-46a013bb70d5?w=600&q=80', description: 'Dive into the Red Sea and explore vibrant coral reefs and diverse marine life.' },
      { name: 'Snorkeling Trip', icon: '🐠', duration: '4 hrs', price: '400 EGP', image: 'https://images.unsplash.com/photo-1583212292454-1fe6229603b7?w=600&q=80', description: 'Snorkel in crystal-clear waters and discover the underwater paradise of the Red Sea.' },
      { name: 'Desert Safari', icon: '🏜️', duration: '6 hrs', price: '600 EGP', image: 'https://images.unsplash.com/photo-1542401299-041cb3f51999?w=600&q=80', description: 'Embark on an exciting desert safari with quad biking and Bedouin dinner.' }
    ]
  },
  { 
    name: 'Sharm El Sheikh', tag: 'City of Peace', img: 'https://images.unsplash.com/photo-1549140600-78c9b8275e9d?w=800&q=80', desc: 'Sinai\'s crown jewel with world-class diving.', flight: 1000,
    activities: [
      { name: 'Ras Mohammed Dive', icon: '🤿', duration: '4 hrs', price: '900 EGP', image: 'https://images.unsplash.com/photo-1549140600-78c9b8275e9d?w=600&q=80', description: 'Dive in one of the world\'s most spectacular marine reserves at Ras Mohammed National Park.' },
      { name: 'Camel Safari', icon: '🐪', duration: '2 hrs', price: '300 EGP', image: 'https://images.unsplash.com/photo-1547234935-80c700199bb0?w=600&q=80', description: 'Ride camels through the Sinai desert and enjoy stunning mountain views.' },
      { name: 'Naama Bay Nightlife', icon: '🌃', duration: 'Flexible', price: 'Free', image: 'https://images.unsplash.com/photo-1514525253161-7a46d19cd819?w=600&q=80', description: 'Experience the vibrant nightlife, restaurants, and entertainment at Naama Bay.' }
    ]
  },
  { 
    name: 'Alexandria', tag: 'Mediterranean Pearl', img: 'images/Alex.jpg', desc: 'The city of Alexander the Great.', flight: 800,
    activities: [
      { name: 'Bibliotheca Alexandrina', icon: '📚', duration: '2-3 hrs', price: '100 EGP', image: 'https://images.unsplash.com/photo-1568322445389-f64ac2515020?w=600&q=80', description: 'Visit the modern revival of the ancient Library of Alexandria, a cultural landmark.' },
      { name: 'Citadel of Qaitbay', icon: '🏰', duration: '2 hrs', price: '120 EGP', image: 'https://images.unsplash.com/photo-1572252009286-268acec5ca0a?w=600&q=80', description: 'Explore the 15th-century fortress built on the site of the ancient Lighthouse of Alexandria.' },
      { name: 'Seafood Dinner', icon: '🦐', duration: '2 hrs', price: '300 EGP', image: 'https://images.unsplash.com/photo-1559339352-11d035aa65de?w=600&q=80', description: 'Enjoy fresh Mediterranean seafood at Alexandria\'s famous waterfront restaurants.' }
    ]
  },
  { 
    name: 'Dahab', tag: 'Sinai Escape', img: 'images/Dahab.jpg', desc: 'Laid-back beach town famous for the Blue Hole.', flight: 1150,
    activities: [
      { name: 'Blue Hole Dive', icon: '🕳️', duration: '3 hrs', price: '600 EGP', image: 'https://images.unsplash.com/photo-1539650116574-75c0c6d73f6e?w=600&q=80', description: 'Dive at the world-famous Blue Hole, one of the most spectacular dive sites in the world.' },
      { name: 'Mount Sinai Hike', icon: '⛰️', duration: '6-8 hrs', price: '400 EGP', image: 'https://images.unsplash.com/photo-1506905925346-21bda4d32df4?w=600&q=80', description: 'Hike Mount Sinai at night to witness a breathtaking sunrise from the summit.' },
      { name: 'Beach Relaxation', icon: '🏖️', duration: 'Flexible', price: 'Free', image: 'https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=600&q=80', description: 'Relax on the beautiful beaches of Dahab and enjoy the laid-back atmosphere.' }
    ]
  }
];

// ============ STATE MANAGEMENT ============
let state = JSON.parse(sessionStorage.getItem('pw_state')) || {
  user: null,
  search: { from: 'Cairo', to: 'Luxor', date: '', passengers: 1, trip: 'oneway' },
  selectedFlight: null,
  selectedSeats: [], 
  passengersDetails: [], 
  luggageSelections: {}, 
  paymentMethod: null,
  sliderIndex: 0
};

let appliedPromoCode = null; 
let selectedPromoOption = null;

function saveState() { sessionStorage.setItem('pw_state', JSON.stringify(state)); }

// ============ INITIALIZATION ============
document.addEventListener('DOMContentLoaded', () => {
  loadUser(); updateUI();
  if (document.querySelector('.hero-slide')) initSlider();
  if (document.getElementById('destGrid') || document.getElementById('destGridFull')) renderDestinations();
  if (document.getElementById('searchDate')) { setDefaultDate(); prefillQuickSearch(); }
  if (document.getElementById('flightList')) initBookingPage();
  if (document.getElementById('seatRows')) renderSeatMap();
  if (document.getElementById('passengersContainer')) renderPassengerForms(); 
  if (document.getElementById('luggageCardsContainer')) renderLuggagePage();  
  if (document.getElementById('checkoutFlight')) renderCheckout();
  if (document.getElementById('bookingsContainer')) renderMyBookings();
  if (document.getElementById('redeemSection')) updateRedeemSection();
  if (document.getElementById('activitiesContainer')) renderActivitiesPage();
  initEventListeners();
});

function setDefaultDate() {
  const today = new Date();
  const nextWeek = new Date(today.getTime() + 7 * 24 * 60 * 60 * 1000);
  const dateStr = nextWeek.toISOString().split('T')[0];
  const dateEl = document.getElementById('searchDate');
  if (dateEl) dateEl.value = dateStr;
  const returnEl = document.getElementById('searchReturn');
  if (returnEl) returnEl.value = new Date(today.getTime() + 14 * 24 * 60 * 60 * 1000).toISOString().split('T')[0];
  state.search.date = dateStr;
}

function prefillQuickSearch() {
  const fromEl = document.getElementById('searchFrom');
  const toEl = document.getElementById('searchTo');
  const paxEl = document.getElementById('searchPax');
  if (fromEl) fromEl.value = state.search.from;
  if (toEl) toEl.value = state.search.to;
  if (paxEl) paxEl.value = state.search.passengers;
}

// ============ NAVIGATION ============
function navigateTo(page) {
  const pageMap = {
    'home': 'index.html', 'booking': 'booking.html', 'seat': 'seat-selection.html',
    'passengers': 'passengers.html', 'luggage': 'luggage.html', 'activities': 'activities.html',
    'loyalty-calc': 'loyalty.html', 'destinations': 'destinations.html',
    'checkout': 'checkout.html', 'login': 'login.html', 'my-bookings': 'my-bookings.html'
  };
  if (pageMap[page]) window.location.href = pageMap[page];
}

function toggleMobileMenu() {
  const nav = document.querySelector('.nav-links');
  if (nav) nav.classList.toggle('open');
}

// ============ HERO SLIDER ============
function initSlider() {
  const slides = document.querySelectorAll('.hero-slide');
  const dotsContainer = document.getElementById('sliderDots');
  if (!slides.length || !dotsContainer) return;
  slides.forEach((_, i) => {
    const dot = document.createElement('div');
    dot.className = 'slider-dot' + (i === state.sliderIndex ? ' active' : '');
    dot.onclick = () => goToSlide(i);
    dotsContainer.appendChild(dot);
  });
  setInterval(() => {
    state.sliderIndex = (state.sliderIndex + 1) % slides.length;
    goToSlide(state.sliderIndex);
  }, 5000);
}

function goToSlide(index) {
  const slides = document.querySelectorAll('.hero-slide');
  const dots = document.querySelectorAll('.slider-dot');
  slides.forEach(s => s.classList.remove('active'));
  dots.forEach(d => d.classList.remove('active'));
  slides[index].classList.add('active');
  dots[index].classList.add('active');
  state.sliderIndex = index;
}

// ============ DESTINATIONS ============
function renderDestinations() {
  const grid = document.getElementById('destGrid') || document.getElementById('destGridFull');
  if (!grid) return;
  
  grid.innerHTML = DESTINATIONS.map(d => `
    <div class="dest-card">
      <div class="dest-img">
        <img src="${d.img}" alt="${d.name}" loading="lazy" />
        <div class="dest-tag">${d.tag}</div>
      </div>
      <div class="dest-body">
        <h3>${d.name}</h3>
        <div class="dest-meta">✈ From ${d.flight} EGP</div>
        <p>${d.desc}</p>
        
        <div style="display: flex; gap: 0.75rem; margin-top: 1rem;">
          <button class="btn btn-primary" onclick="bookDestination('${d.name}')" style="flex: 1;">Book Flight →</button>
          <button class="btn btn-ghost" onclick="viewActivities('${d.name}')" style="flex: 1;">Show Activities 🎯</button>
        </div>
      </div>
    </div>
  `).join('');
}

function bookDestination(city) {
  if (!state.user) { alert('Please login or register first to book a flight.'); navigateTo('login'); return; }
  state.search.to = city; state.search.from = 'Cairo';
  saveState(); navigateTo('booking');
}

// ============ FLIGHT SEARCH & BOOKING PAGE ============
function initBookingPage() {
  const savedFlights = sessionStorage.getItem('pw_current_flights');
  if (savedFlights) {
    window.currentFlights = JSON.parse(savedFlights);
    renderFlights(window.currentFlights);
    const sub = document.getElementById('bookingSubtitle');
    if (sub) sub.textContent = `${state.search.from} → ${state.search.to} • ${state.search.passengers} Passenger(s) • ${formatDate(state.search.date)}`;
  }
}

function searchFlights() {
  const fromEl = document.getElementById('searchFrom');
  const toEl = document.getElementById('searchTo');
  const dateEl = document.getElementById('searchDate');
  const paxEl = document.getElementById('searchPax');
  if (!fromEl || !toEl) return;
  state.search.from = fromEl.value; state.search.to = toEl.value;
  state.search.date = dateEl.value; state.search.passengers = parseInt(paxEl.value);
  if (state.search.from === state.search.to) { alert('Origin and destination cannot be the same'); return; }
  saveState();
  const list = document.getElementById('flightList');
  if (list) list.innerHTML = '<div style="text-align:center;padding:3rem;"><div class="loading" style="height:200px;border-radius:var(--radius)"></div><p style="margin-top:1rem;color:var(--muted)">Searching for the best flights...</p></div>';
  setTimeout(() => {
    const flights = generateFlights(state.search.from, state.search.to);
    window.currentFlights = flights;
    sessionStorage.setItem('pw_current_flights', JSON.stringify(flights));
    resetFilters(); navigateTo('booking');
  }, 800);
}

function generateFlights(from, to) {
  const baseDistance = getDistance(from, to);
  const basePrice = Math.round(baseDistance * 1.8);
  const flights = [];
  const times = ['06:30', '09:15', '12:00', '15:45', '18:30', '21:00'];
  const durations = [60, 75, 90];
  times.forEach((t, i) => {
    const duration = durations[i % durations.length];
    const arrivalTime = addMinutes(t, duration);
    const price = basePrice + (i * 100) + Math.floor(Math.random() * 150);
    flights.push({ id: i, number: 'PW-' + (200 + i), from, to, departure: t, arrival: arrivalTime, duration: duration + ' min', durationMin: duration, price: price, class: i < 2 ? 'business' : 'economy', seats: Math.floor(Math.random() * 40) + 20 });
  });
  return flights;
}

function renderFlights(flights) {
  const list = document.getElementById('flightList');
  const count = document.getElementById('flightCount');
  const message = document.getElementById('flightResultsMessage');
  if (!list) return;
  if (!flights || flights.length === 0) { list.innerHTML = ''; if (message) message.style.display = 'block'; if (count) count.textContent = '(0)'; return; }
  if (message) message.style.display = 'none';
  if (count) count.textContent = `(${flights.length})`;
  list.innerHTML = flights.map(f => `
    <div class="flight-card ${f.class === 'business' ? 'business-class' : ''}" data-class="${f.class}" data-price="${f.price}" data-duration="${f.durationMin}" data-departure="${f.departure}">
      <div class="airline-info"><div class="airline-logo">𓆙</div><div><div class="airline-name">PharaohWings</div><div class="airline-code">${f.number} • ${f.class === 'business' ? '<span class="badge badge-business">Business</span>' : '<span class="badge badge-economy">Economy</span>'}</div></div></div>
      <div class="flight-times">
        <div class="time-block"><div class="time">${f.departure}</div><div class="city">${f.from}</div></div>
        <div style="flex:1"><div class="flight-line"></div><div class="flight-duration">${f.duration}</div></div>
        <div class="time-block"><div class="time">${f.arrival}</div><div class="city">${f.to}</div></div>
      </div>
      <div class="flight-price"><div class="price-amount">${f.price}</div><div class="price-currency">EGP</div><button class="btn btn-primary" onclick="selectFlight(${f.id})">Select Seat →</button></div>
    </div>
  `).join('');
  window.currentFlights = flights;
}

function applyFilters() {
  if (!window.currentFlights) return;
  const classFilter = document.getElementById('filterClass')?.value || 'all';
  const sortFilter = document.getElementById('filterSort')?.value || 'price';
  const priceFilter = parseInt(document.getElementById('filterPrice')?.value || 3000);
  let filtered = [...window.currentFlights];
  if (classFilter !== 'all') filtered = filtered.filter(f => f.class === classFilter);
  filtered = filtered.filter(f => f.price <= priceFilter);
  switch(sortFilter) {
    case 'price': filtered.sort((a, b) => a.price - b.price); break;
    case 'price-desc': filtered.sort((a, b) => b.price - a.price); break;
    case 'duration': filtered.sort((a, b) => a.durationMin - b.durationMin); break;
    case 'departure': filtered.sort((a, b) => a.departure.localeCompare(b.departure)); break;
  }
  renderFlights(filtered);
}

function resetFilters() {
  const fc = document.getElementById('filterClass'); const fs = document.getElementById('filterSort');
  const fp = document.getElementById('filterPrice'); const pv = document.getElementById('priceValue');
  if (fc) fc.value = 'all'; if (fs) fs.value = 'price';
  if (fp) { fp.value = 3000; } if (pv) pv.textContent = '3000 EGP';
  if (window.currentFlights) renderFlights(window.currentFlights);
}

function toggleView(view) {
  const list = document.getElementById('flightList');
  const btnList = document.getElementById('viewList'); const btnGrid = document.getElementById('viewGrid');
  if (!list) return;
  if (view === 'grid') { list.classList.add('flight-grid'); if (btnGrid) btnGrid.classList.add('active'); if (btnList) btnList.classList.remove('active'); } 
  else { list.classList.remove('flight-grid'); if (btnList) btnList.classList.add('active'); if (btnGrid) btnGrid.classList.remove('active'); }
}

function selectFlight(id) {
  if (!state.user) { alert('Please login or register first to proceed with booking.'); navigateTo('login'); return; }
  state.selectedFlight = window.currentFlights.find(f => f.id === id);
  state.selectedSeats = []; saveState(); navigateTo('seat');
}

// ============ SEAT MAP ============
function renderSeatMap() {
  const f = state.selectedFlight;
  if (!f) { navigateTo('booking'); return; }
  const sub = document.getElementById('seatSubtitle');
  if (sub) sub.textContent = `Flight ${f.number} • ${f.from} → ${f.to} • ${f.departure}`;
  const container = document.getElementById('seatRows');
  if (!container) return; container.innerHTML = '';
  const rows = 12; const seatsPerRow = ['A', 'B', 'C', 'aisle', 'D', 'E', 'F'];
  const takenSeats = new Set();
  for (let i = 0; i < 25; i++) {
    const r = Math.floor(Math.random() * rows) + 1;
    const s = seatsPerRow[Math.floor(Math.random() * 6)];
    if (s !== 'aisle') takenSeats.add(r + s);
  }
  for (let r = 1; r <= rows; r++) {
    const row = document.createElement('div'); row.className = 'seat-row';
    seatsPerRow.forEach(s => {
      if (s === 'aisle') { const aisle = document.createElement('div'); aisle.className = 'aisle'; row.appendChild(aisle); } 
      else {
        const seat = document.createElement('div');
        const isBusiness = r <= 3; const seatId = r + s; const isTaken = takenSeats.has(seatId);
        const isSelected = state.selectedSeats.some(sel => sel.id === seatId); 
        seat.className = 'seat' + (isBusiness ? ' business' : '') + (isTaken ? ' taken' : '') + (isSelected ? ' selected' : '');
        seat.textContent = s;
        if (!isTaken) seat.onclick = () => toggleSeat(seat, seatId, isBusiness);
        row.appendChild(seat);
      }
    });
    container.appendChild(row);
  }
  updateSeatSummary();
}

function toggleSeat(el, seatId, isBusiness) {
  const maxSeats = state.search.passengers;
  if (el.classList.contains('selected')) {
    el.classList.remove('selected');
    state.selectedSeats = state.selectedSeats.filter(s => s.id !== seatId);
  } else {
    if (state.selectedSeats.length >= maxSeats) { alert(`You can only select ${maxSeats} seat(s) for ${maxSeats} passenger(s). Please deselect a seat first if you want to change.`); return; }
    el.classList.add('selected');
    state.selectedSeats.push({ id: seatId, class: isBusiness ? 'business' : 'economy', row: el.textContent });
  }
  saveState(); updateSeatSummary();
}

function updateSeatSummary() {
  const f = state.selectedFlight; const container = document.getElementById('seatSummary');
  if (!container || !f) return;
  const maxSeats = state.search.passengers;
  if (state.selectedSeats.length === 0) { container.innerHTML = `<p style="color:var(--muted);text-align:center;padding:1rem 0">Please select ${maxSeats} seat(s)</p>`; return; }
  let totalBaseFare = 0, totalSeatUpgrade = 0;
  let seatsHtml = state.selectedSeats.map(seat => {
    const seatPrice = seat.class === 'business' ? 800 : 0;
    totalBaseFare += f.price; totalSeatUpgrade += seatPrice;
    return `<div class="summary-row"><span class="label">Seat ${seat.id} (${seat.class})</span><span class="value">${f.price + seatPrice} EGP</span></div>`;
  }).join('');
  const taxes = Math.round(totalBaseFare * 0.14); const total = totalBaseFare + totalSeatUpgrade + taxes;
  container.innerHTML = `
    <div class="summary-row"><span class="label">Flight</span><span class="value">${f.number}</span></div>
    <div class="summary-row"><span class="label">Route</span><span class="value">${f.from} → ${f.to}</span></div>
    <div style="margin: 0.5rem 0; border-top: 1px dashed #e0e0e0;"></div>${seatsHtml}
    <div class="summary-row"><span class="label">Taxes (14%)</span><span class="value">${taxes} EGP</span></div>
    <div class="summary-row total"><span class="label">Total (${state.selectedSeats.length} pax)</span><span class="value">${total} EGP</span></div>
    <p style="font-size:0.8rem; color:var(--success); margin-top:0.5rem; text-align:center; font-weight:600;">Selected ${state.selectedSeats.length} of ${maxSeats} required seats</p>`;
}

function proceedToPassengerDetails() {
  const maxSeats = state.search.passengers;
  if (state.selectedSeats.length < maxSeats) { alert(`Please select exactly ${maxSeats} seat(s) for ${maxSeats} passenger(s) before proceeding.`); return; }
  saveState(); navigateTo('passengers');
}

// ============ PASSENGER DETAILS PAGE ============
function renderPassengerForms() {
  const container = document.getElementById('passengersContainer');
  if (!container) return;
  let html = '';
  for (let i = 0; i < state.search.passengers; i++) {
    const pax = state.passengersDetails[i] || {};
    html += `
      <div class="checkout-card">
        <h3>👤 Passenger ${i + 1} <span style="font-size:0.9rem; color:var(--gold); font-weight:400;">(Seat: ${state.selectedSeats[i]?.id || 'TBD'})</span></h3>
        <div class="pay-form-row">
          <div><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">First Name</label><input type="text" class="pay-input pax-fname" data-idx="${i}" value="${pax.firstName || ''}" required /></div>
          <div><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">Last Name</label><input type="text" class="pay-input pax-lname" data-idx="${i}" value="${pax.lastName || ''}" required /></div>
        </div>
        <div class="pay-form-row">
          <div><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">Date of Birth</label><input type="date" class="pay-input pax-dob" data-idx="${i}" value="${pax.dob || ''}" required /></div>
          <div><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">Gender</label>
            <select class="pay-input pax-gender" data-idx="${i}" required>
              <option value="">Select</option>
              <option value="Male" ${pax.gender === 'Male' ? 'selected' : ''}>Male</option>
              <option value="Female" ${pax.gender === 'Female' ? 'selected' : ''}>Female</option>
            </select>
          </div>
        </div>
        <div class="pay-form-row">
          <div><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">Nationality</label><input type="text" class="pay-input pax-nat" data-idx="${i}" value="${pax.nationality || 'Egyptian'}" required /></div>
          <div><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">Passport / ID Number</label><input type="text" class="pay-input pax-pass" data-idx="${i}" value="${pax.passportNo || ''}" required /></div>
        </div>
        <div class="pay-form-row">
          <div><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">Passport Expiry Date</label><input type="date" class="pay-input pax-exp" data-idx="${i}" value="${pax.passportExpiry || ''}" required /></div>
        </div>
      </div>
    `;
  }
  container.innerHTML = html;
}

function savePassengersAndNext(e) {
  e.preventDefault();
  const details = [];
  for (let i = 0; i < state.search.passengers; i++) {
    details.push({
      firstName: document.querySelector(`.pax-fname[data-idx="${i}"]`).value,
      lastName: document.querySelector(`.pax-lname[data-idx="${i}"]`).value,
      dob: document.querySelector(`.pax-dob[data-idx="${i}"]`).value,
      gender: document.querySelector(`.pax-gender[data-idx="${i}"]`).value,
      nationality: document.querySelector(`.pax-nat[data-idx="${i}"]`).value,
      passportNo: document.querySelector(`.pax-pass[data-idx="${i}"]`).value,
      passportExpiry: document.querySelector(`.pax-exp[data-idx="${i}"]`).value,
      seatId: state.selectedSeats[i].id
    });
  }
  state.passengersDetails = details;
  state.passengersDetails.forEach(p => { if (!state.luggageSelections[p.seatId]) state.luggageSelections[p.seatId] = 'none'; });
  saveState(); navigateTo('luggage');
}

// ============ LUGGAGE PAGE ============
function renderLuggagePage() {
  const container = document.getElementById('luggageCardsContainer');
  if (!container) return;
  let html = '';
  state.passengersDetails.forEach((pax) => {
    const currentBaggage = state.luggageSelections[pax.seatId] || 'none';
    html += `
      <div class="checkout-card">
        <h3>🧳 Luggage for: <span style="color:var(--gold)">${pax.firstName} ${pax.lastName}</span> <span style="font-size:0.85rem; color:var(--muted);">(Seat: ${pax.seatId})</span></h3>
        <div class="pay-form-row">
          <div style="flex:1">
            <label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">Extra Baggage Allowance</label>
            <select class="pay-input luggage-select" data-seat="${pax.seatId}" onchange="updateLuggageSelection(this)">
              ${BAGGAGE_OPTIONS.map(opt => `<option value="${opt.id}" ${currentBaggage === opt.id ? 'selected' : ''}>${opt.label} (+${opt.price} EGP)</option>`).join('')}
            </select>
          </div>
        </div>
      </div>
    `;
  });
  container.innerHTML = html;
  updateBookingSummary('luggageSummary');
}

function updateLuggageSelection(selectEl) {
  const seatId = selectEl.dataset.seat;
  state.luggageSelections[seatId] = selectEl.value;
  saveState(); updateBookingSummary('luggageSummary');
}

// ============ UNIFIED BOOKING SUMMARY ============
function updateBookingSummary(targetElementId) {
  const f = state.selectedFlight;
  if (!f || state.selectedSeats.length === 0) return;
  let totalBaseFare = 0, totalSeatUpgrade = 0, totalLuggage = 0;
  state.selectedSeats.forEach(seat => {
    totalBaseFare += f.price;
    totalSeatUpgrade += (seat.class === 'business' ? 800 : 0);
    const bagOpt = BAGGAGE_OPTIONS.find(b => b.id === (state.luggageSelections[seat.id] || 'none'));
    totalLuggage += bagOpt ? bagOpt.price : 0;
  });
  const taxes = Math.round(totalBaseFare * 0.14);
  const user = state.user;
  const loyaltyDiscount = user ? Math.min(Math.floor(user.points / 100), Math.round((totalBaseFare + totalSeatUpgrade + totalLuggage) * 0.25)) : 0;
  const promoDiscount = appliedPromoCode ? appliedPromoCode.discount : 0;
  const total = totalBaseFare + totalSeatUpgrade + totalLuggage + taxes - loyaltyDiscount - promoDiscount;
  const sumDiv = document.getElementById(targetElementId);
  if (sumDiv) {
    sumDiv.innerHTML = `
      <div class="summary-row"><span class="label">Base Fare (${state.selectedSeats.length} x ${f.price})</span><span class="value">${totalBaseFare} EGP</span></div>
      <div class="summary-row"><span class="label">Seat Upgrades</span><span class="value">${totalSeatUpgrade} EGP</span></div>
      <div class="summary-row"><span class="label">Extra Luggage</span><span class="value">${totalLuggage} EGP</span></div>
      <div class="summary-row"><span class="label">Taxes (14%)</span><span class="value">${taxes} EGP</span></div>
      ${loyaltyDiscount > 0 ? `<div class="summary-row"><span class="label" style="color:var(--success)">Loyalty Discount</span><span class="value" style="color:var(--success)">-${loyaltyDiscount} EGP</span></div>` : ''}
      ${promoDiscount > 0 ? `<div class="summary-row"><span class="label" style="color:var(--success)">Promo Code (${appliedPromoCode.code})</span><span class="value" style="color:var(--success)">-${promoDiscount} EGP</span></div>` : ''}
      <div class="summary-row total"><span class="label">Total</span><span class="value">${total} EGP</span></div>`;
  }
}

// ============ CHECKOUT ============
function renderCheckout() {
  const f = state.selectedFlight;
  if (!f || state.selectedSeats.length === 0) { navigateTo('booking'); return; }
  updateBookingSummary('checkoutSummary');
  if (state.user) {
    const e = document.getElementById('contactEmail'); if(e) e.value = state.user.email || '';
    const p = document.getElementById('contactPhone'); if(p) p.value = state.user.phone || '';
  }
}

// ============ EVENT LISTENERS ============
function initEventListeners() {
  document.querySelectorAll('.trip-toggle button').forEach(btn => {
    btn.onclick = () => {
      document.querySelectorAll('.trip-toggle button').forEach(b => b.classList.remove('active'));
      btn.classList.add('active'); state.search.trip = btn.dataset.trip;
      const ret = document.getElementById('returnField');
      if (ret) ret.style.display = btn.dataset.trip === 'round' ? 'flex' : 'none';
    };
  });
  document.querySelectorAll('.pay-method').forEach(m => {
    m.onclick = () => {
      document.querySelectorAll('.pay-method').forEach(x => x.classList.remove('active'));
      m.classList.add('active'); state.paymentMethod = m.dataset.method;
      renderPaymentDetails(m.dataset.method);
    };
  });
  document.querySelectorAll('.auth-tab').forEach(tab => {
    tab.onclick = () => {
      document.querySelectorAll('.auth-tab').forEach(t => t.classList.remove('active'));
      document.querySelectorAll('.auth-form').forEach(f => f.classList.remove('active'));
      tab.classList.add('active');
      const form = document.getElementById(tab.dataset.tab + 'Form');
      if (form) form.classList.add('active');
    };
  });
  window.addEventListener('scroll', () => {
    const header = document.getElementById('siteHeader');
    if (header) header.classList.toggle('scrolled', window.scrollY > 50);
  });
  const modal = document.getElementById('authModal');
  if (modal) modal.onclick = (e) => { if (e.target.id === 'authModal') closeAuthModal(); };
}

// =========================================================
// PAYMENT DETAILS
// =========================================================
function renderPaymentDetails(method) {
  const container = document.getElementById('payDetails');
  if (!container) return;
  const templates = {
    instapay: `<div style="display:flex; align-items:center; gap:10px; margin-bottom:1rem;"><img src="images/instapay.png" alt="InstaPay" style="height:30px; object-fit:contain;" /><h4 style="margin:0;">InstaPay Payment</h4></div><div class="pay-form-row"><div><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">InstaPay Username / Phone</label><input type="text" class="pay-input" placeholder="01xxxxxxxxx" /></div><div><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">Bank</label><select class="pay-input"><option>NBE (البنك الأهلي)</option><option>CIB</option><option>Banque Misr (مصر)</option><option>QNB Alahli</option></select></div></div><p style="font-size:.85rem;color:var(--slate);margin-top:1rem; background:var(--sand); padding:1rem; border-radius:8px; border-left: 3px solid var(--gold);">ℹ️ You will be redirected to the InstaPay app to complete the payment securely.</p>`,
    fawry: `<div style="display:flex; align-items:center; gap:10px; margin-bottom:1rem;"><img src="images/fawrypng.png" alt="Fawry" style="height:30px; object-fit:contain;" /><h4 style="margin:0;">Pay at Fawry</h4></div><p style="margin-bottom:1rem;color:var(--slate); font-size:0.9rem;">A reference code has been generated. Visit any Fawry outlet or use the Fawry app to pay within 48 hours.</p><div class="fawry-code"><div style="font-size:0.9rem; opacity:0.8; letter-spacing:1px;">REFERENCE CODE</div><div class="code" style="font-size:2.2rem; letter-spacing:6px; margin:0.5rem 0; font-weight:700;">${generateFawryCode()}</div><small>Valid for 48 hours • Keep this code safe</small></div>`,
    wallet: `<h4 style="margin-bottom:1rem; display:flex; align-items:center; gap:8px;"><img src="https://cdn-icons-png.flaticon.com/512/2331/2331941.png" style="height:24px;" /> Mobile Wallet</h4><div class="wallet-grid"><div class="wallet-option vodafone" onclick="selectWallet(this, 'Vodafone Cash')"><img src="images/vodafon.jpg" alt="Vodafone" class="wallet-logo" /><div>Vodafone Cash</div></div><div class="wallet-option orange" onclick="selectWallet(this, 'Orange Cash')"><img src="images/orange.png" alt="Orange" class="wallet-logo" /><div>Orange Cash</div></div><div class="wallet-option etisalat" onclick="selectWallet(this, 'Etisalat Cash')"><img src="images/Etisalat.png" alt="Etisalat" class="wallet-logo" /><div>Etisalat Cash</div></div><div class="wallet-option we" onclick="selectWallet(this, 'WE Pay')"><img src="images/we.png" alt="WE" class="wallet-logo" /><div>WE Pay</div></div></div><div id="walletForm" style="margin-top:1.5rem; display:none; animation: fadeIn 0.3s ease-in-out;"><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">Wallet Phone Number</label><input type="tel" class="pay-input" id="walletPhone" placeholder="01xxxxxxxxx" style="margin-bottom: 1rem;" /><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem; color:var(--navy);">🔐 Enter OTP (Sent to your phone)</label><input type="text" class="pay-input" id="otpInput" placeholder="1 2 3 4 5 6" maxlength="6" style="letter-spacing: 8px; text-align: center; font-size: 1.4rem; font-weight: bold; border: 2px solid var(--gold); background: var(--sand);" oninput="this.value = this.value.replace(/[^0-9]/g, '')" /><p style="font-size:.8rem;color:var(--muted);margin-top:.75rem; display:flex; align-items:center; gap:5px;"><span>🔒</span> Secure 2-Factor Authentication. Code expires in 05:00 mins.</p></div>`,
    card: `<div style="display:flex; align-items:center; gap:10px; margin-bottom:1rem;"><img src="https://upload.wikimedia.org/wikipedia/commons/thumb/2/2a/Mastercard-logo.svg/1280px-Mastercard-logo.svg.png" alt="Mastercard" style="height:25px; object-fit:contain;" /><img src="images/visa.jpg" alt="Visa" style="height:25px; object-fit:contain;" /><h4 style="margin:0;">Credit / Debit Card</h4></div><div class="pay-form-row full"><div><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">Card Number</label><input type="text" class="pay-input" id="cardNumber" placeholder="1234 5678 9012 3456" maxlength="19" oninput="this.value = this.value.replace(/[^0-9]/g, '').replace(/(.{4})/g, '$1 ').trim()" /></div></div><div class="pay-form-row"><div><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">Expiry Date</label><input type="text" class="pay-input" placeholder="MM/YY" maxlength="5" oninput="if(this.value.length==2 && !this.value.includes('/')) this.value+='/'" /></div><div><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">CVV</label><input type="password" class="pay-input" placeholder="123" maxlength="4" /></div></div><div class="pay-form-row full"><div><label style="font-size:.85rem;font-weight:600;display:block;margin-bottom:.4rem">Cardholder Name</label><input type="text" class="pay-input" placeholder="AHMED MOHAMED" style="text-transform:uppercase;" /></div></div>`
  };
  container.innerHTML = templates[method] || '<p style="color:var(--muted);text-align:center;padding:2rem 0">Select a payment method above</p>';
}

function selectWallet(el, name) {
  document.querySelectorAll('.wallet-option').forEach(w => w.classList.remove('active'));
  el.classList.add('active');
  const wf = document.getElementById('walletForm');
  if (wf) { wf.style.display = 'block'; const phoneInput = document.getElementById('walletPhone'); if(phoneInput) setTimeout(() => phoneInput.focus(), 100); }
}

function generateFawryCode() { return Math.floor(10000000 + Math.random() * 90000000).toString(); }

// =========================================================
// PROMO CODE SYSTEM
// =========================================================
function updateRedeemSection() {
  const redeemSection = document.getElementById('redeemSection');
  if (!redeemSection) return;
  const redeemLoginMessage = document.getElementById('redeemLoginMessage');
  const redeemContent = document.getElementById('redeemContent');
  const balanceEl = document.getElementById('redeemPointsBalance');
  const myPromoSection = document.getElementById('myPromoCodesSection');
  
  if (!state.user) {
    redeemLoginMessage.style.display = 'block'; redeemContent.style.display = 'none';
    if(myPromoSection) myPromoSection.style.display = 'none'; return;
  }
  redeemLoginMessage.style.display = 'none'; redeemContent.style.display = 'block';
  if (balanceEl) balanceEl.textContent = state.user.points.toLocaleString();
  
  const userPromoCodes = getUserPromoCodes();
  const activeCodes = userPromoCodes.filter(c => !c.used);
  if (activeCodes.length > 0 && myPromoSection) {
    myPromoSection.style.display = 'block';
    document.getElementById('myPromoCodesList').innerHTML = activeCodes.map(code => `
      <div style="background: var(--sand); padding: 1rem; border-radius: var(--radius-sm); margin-bottom: 0.75rem; display: flex; justify-content: space-between; align-items: center; border: 1px solid var(--sand-2);">
        <div>
          <div style="font-family: var(--font-head); font-weight: 700; color: var(--navy); font-size: 1.1rem; letter-spacing: 2px;">${code.code}</div>
          <div style="font-size: 0.85rem; color: var(--muted); margin-top: 0.25rem;">${code.discount} EGP discount • Expires: ${formatDate(code.expiresAt)}</div>
        </div>
        <button class="btn btn-ghost" style="padding: 0.4rem 1rem; font-size: 0.8rem;" onclick="copySpecificCode('${code.code}')">📋 Copy</button>
      </div>
    `).join('');
  } else if (myPromoSection) { myPromoSection.style.display = 'none'; }
}

function selectPromoOption(el, points, discount) {
  document.querySelectorAll('.promo-option').forEach(opt => { opt.style.border = '2px solid var(--sand-2)'; opt.style.background = 'var(--white)'; });
  el.style.border = '2px solid var(--gold)'; el.style.background = 'rgba(212, 175, 55, 0.08)';
  selectedPromoOption = { points, discount };
}

function generateRandomCode(length = 6) {
  const chars = 'ABCDEFGHJKLMNPQRSTUVWXYZ23456789';
  let result = 'PHARAOH-';
  for (let i = 0; i < length; i++) result += chars.charAt(Math.floor(Math.random() * chars.length));
  return result;
}

function generatePromoCode() {
  if (!state.user) { alert('Please login first.'); navigateTo('login'); return; }
  if (!selectedPromoOption) { alert('Please select a discount value first.'); return; }
  if (state.user.points < selectedPromoOption.points) { alert(`You need ${selectedPromoOption.points} points. Your balance: ${state.user.points}.`); return; }
  if (!confirm(`Convert ${selectedPromoOption.points} points into a ${selectedPromoOption.discount} EGP promo code?`)) return;
  
  state.user.points -= selectedPromoOption.points;
  const promoCode = { code: generateRandomCode(), discount: selectedPromoOption.discount, pointsUsed: selectedPromoOption.points, createdAt: new Date().toISOString(), expiresAt: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString(), used: false, userId: state.user.email };
  savePromoCode(promoCode); saveUser(); updateUI(); updateRedeemSection();
  
  const container = document.getElementById('generatedCodeContainer');
  const codeEl = document.getElementById('generatedCode');
  const detailsEl = document.getElementById('generatedCodeDetails');
  if (container && codeEl && detailsEl) {
    codeEl.textContent = promoCode.code; detailsEl.textContent = `${promoCode.discount} EGP discount • Valid for 30 days`; 
    container.style.display = 'block'; container.scrollIntoView({ behavior: 'smooth', block: 'center' });
  }
  alert(`✅ Promo Code Generated!\n\nCode: ${promoCode.code}\nDiscount: ${promoCode.discount} EGP`);
  selectedPromoOption = null;
  document.querySelectorAll('.promo-option').forEach(opt => { opt.style.border = '2px solid var(--sand-2)'; opt.style.background = 'var(--white)'; });
}

function savePromoCode(promoCode) {
  const allCodes = JSON.parse(localStorage.getItem('pw_promo_codes') || '[]');
  allCodes.push(promoCode); localStorage.setItem('pw_promo_codes', JSON.stringify(allCodes));
}

function getUserPromoCodes() {
  if (!state.user) return [];
  return JSON.parse(localStorage.getItem('pw_promo_codes') || '[]').filter(c => c.userId === state.user.email);
}

function copyPromoCode() {
  const codeEl = document.getElementById('generatedCode');
  if (codeEl) navigator.clipboard.writeText(codeEl.textContent).then(() => alert('✅ Code copied!'));
}

function copySpecificCode(code) {
  navigator.clipboard.writeText(code).then(() => alert(`✅ Code ${code} copied!`));
}

function applyPromoCode() {
  const input = document.getElementById('promoCodeInput');
  const messageEl = document.getElementById('promoCodeMessage');
  const appliedDetails = document.getElementById('appliedPromoDetails');
  if (!input || !messageEl) return;
  const code = input.value.trim().toUpperCase();
  if (!code) { messageEl.style.display = 'block'; messageEl.style.color = 'var(--danger)'; messageEl.textContent = 'Please enter a promo code.'; return; }
  
  const allCodes = JSON.parse(localStorage.getItem('pw_promo_codes') || '[]');
  const promoCode = allCodes.find(c => c.code === code);
  if (!promoCode) { messageEl.style.display = 'block'; messageEl.style.color = 'var(--danger)'; messageEl.textContent = '❌ Invalid promo code.'; appliedPromoCode = null; updateBookingSummary('checkoutSummary'); return; }
  if (new Date(promoCode.expiresAt) < new Date()) { messageEl.style.display = 'block'; messageEl.style.color = 'var(--danger)'; messageEl.textContent = '❌ This promo code has expired.'; appliedPromoCode = null; updateBookingSummary('checkoutSummary'); return; }
  if (promoCode.used) { messageEl.style.display = 'block'; messageEl.style.color = 'var(--danger)'; messageEl.textContent = '❌ This promo code has already been used.'; appliedPromoCode = null; updateBookingSummary('checkoutSummary'); return; }
  if (state.user && promoCode.userId !== state.user.email) { messageEl.style.display = 'block'; messageEl.style.color = 'var(--danger)'; messageEl.textContent = '❌ This promo code belongs to another account.'; appliedPromoCode = null; updateBookingSummary('checkoutSummary'); return; }
  
  appliedPromoCode = promoCode;
  messageEl.style.display = 'block'; messageEl.style.color = 'var(--success)'; messageEl.textContent = `✅ Promo code applied! You get ${promoCode.discount} EGP discount.`;
  const appliedCodeEl = document.getElementById('appliedPromoCode');
  const appliedDiscountEl = document.getElementById('appliedPromoDiscount');
  if (appliedCodeEl) appliedCodeEl.textContent = promoCode.code;
  if (appliedDiscountEl) appliedDiscountEl.textContent = `${promoCode.discount} EGP discount applied`;
  appliedDetails.style.display = 'block'; input.value = ''; updateBookingSummary('checkoutSummary');
}

function removePromoCode() {
  appliedPromoCode = null;
  const messageEl = document.getElementById('promoCodeMessage');
  const appliedDetails = document.getElementById('appliedPromoDetails');
  if (messageEl) messageEl.style.display = 'none';
  if (appliedDetails) appliedDetails.style.display = 'none';
  updateBookingSummary('checkoutSummary');
}

// =========================================================
// COMPLETE BOOKING
// =========================================================
function completeBooking() {
  const contactEmail = document.getElementById('contactEmail')?.value.trim();
  const contactPhone = document.getElementById('contactPhone')?.value.trim();
  if (!contactEmail || !contactPhone) { alert('Please fill in the Primary Contact Email and Phone.'); return; }
  if (!state.paymentMethod) { alert('Please select a payment method'); return; }
  if (state.paymentMethod === 'wallet') {
    const otp = document.getElementById('otpInput')?.value;
    if (!otp || otp.length < 4) { alert('Please enter a valid OTP code to verify your wallet.'); return; }
  }

  let totalBaseFare = 0, totalSeatUpgrade = 0, totalLuggage = 0;
  state.selectedSeats.forEach(seat => {
    totalBaseFare += state.selectedFlight.price;
    totalSeatUpgrade += (seat.class === 'business' ? 800 : 0);
    const bagOpt = BAGGAGE_OPTIONS.find(b => b.id === (state.luggageSelections[seat.id] || 'none'));
    totalLuggage += bagOpt ? bagOpt.price : 0;
  });

  const loyaltyDiscountInEGP = state.user ? Math.min(Math.floor(state.user.points / 100), Math.round((totalBaseFare + totalSeatUpgrade + totalLuggage) * 0.25)) : 0;
  const loyaltyPointsToDeduct = loyaltyDiscountInEGP * 100;
  if (state.user && loyaltyPointsToDeduct > 0) { state.user.points = Math.max(0, state.user.points - loyaltyPointsToDeduct); }

  if (appliedPromoCode) {
    const allCodes = JSON.parse(localStorage.getItem('pw_promo_codes') || '[]');
    const codeIndex = allCodes.findIndex(c => c.code === appliedPromoCode.code);
    if (codeIndex !== -1) {
      allCodes[codeIndex].used = true; allCodes[codeIndex].usedAt = new Date().toISOString();
      allCodes[codeIndex].usedByBooking = state.selectedFlight.number;
      localStorage.setItem('pw_promo_codes', JSON.stringify(allCodes));
    }
  }

  const pointsPerTicket = calculatePointsForRoute(state.selectedFlight.from, state.selectedFlight.to);
  const bookingRef = 'PW-' + Math.floor(100000 + Math.random() * 900000);

  if (state.user) {
    if (!state.user.tickets) state.user.tickets = [];
    state.passengersDetails.forEach(pax => {
      state.user.points += pointsPerTicket;
      const bagOpt = BAGGAGE_OPTIONS.find(b => b.id === (state.luggageSelections[pax.seatId] || 'none'));
      state.user.tickets.push({
        bookingRef: bookingRef, flight: state.selectedFlight.number, route: `${state.selectedFlight.from} → ${state.selectedFlight.to}`,
        date: state.search.date, seat: pax.seatId, passenger: pax, luggage: bagOpt.label, luggagePrice: bagOpt.price,
        points: pointsPerTicket, contactEmail: contactEmail, contactPhone: contactPhone,
        promoCodeUsed: appliedPromoCode ? appliedPromoCode.code : null
      });
    });
    saveUser(); updateUI();
  }

  alert(`✅ Booking Confirmed!\nReference: ${bookingRef}\n\n🎉 You earned ${pointsPerTicket * state.search.passengers} Pharaoh Points!`);
  sessionStorage.removeItem('pw_state'); sessionStorage.removeItem('pw_current_flights');
  state.selectedFlight = null; state.selectedSeats = []; state.passengersDetails = [];
  state.luggageSelections = {}; appliedPromoCode = null;
  navigateTo('my-bookings');
}

// ============ LOYALTY CALCULATOR ============
function calculatePoints() {
  const from = document.getElementById('calcFrom')?.value; const to = document.getElementById('calcTo')?.value;
  if (!from || !to) return; if (from === to) { alert('Please select different cities'); return; }
  const points = calculatePointsForRoute(from, to); const discount = Math.floor(points / 100);
  const pe = document.getElementById('pointsEarned'); const de = document.getElementById('discountEquiv'); const cr = document.getElementById('calcResult');
  if (pe) pe.textContent = points; if (de) de.textContent = `≈ ${discount} EGP discount on future bookings`; if (cr) cr.style.display = 'block';
}
function calculatePointsForRoute(from, to) { const distance = getDistance(from, to); return Math.round((distance / 100) * POINTS_PER_100KM); }
function getDistance(from, to) { const key1 = `${from}-${to}`; const key2 = `${to}-${from}`; return DISTANCES[key1] || DISTANCES[key2] || 400; }

// ============ AUTH ============
function openAuthModal() { if (state.user) { if (confirm(`Logged in as ${state.user.name}\nPoints: ${state.user.points}\n\nLogout?`)) logout(); return; } navigateTo('login'); }
function closeAuthModal() { const modal = document.getElementById('authModal'); if (modal) modal.classList.remove('active'); else navigateTo('home'); }

function handleLogin(e) {
  e.preventDefault(); const email = document.getElementById('loginEmail').value.trim(); const password = document.getElementById('loginPassword').value;
  let valid = true; const emailGroup = document.getElementById('loginEmail').parentElement; const passGroup = document.getElementById('loginPassword').parentElement;
  if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) { emailGroup.classList.add('error'); valid = false; } else emailGroup.classList.remove('error');
  if (password.length < 6) { passGroup.classList.add('error'); valid = false; } else passGroup.classList.remove('error');
  if (!valid) return;
  const users = JSON.parse(localStorage.getItem('pw_users') || '{}');
  if (users[email]) state.user = users[email];
  else { state.user = { email, name: email.split('@')[0], phone: '', points: 250, tickets: [] }; users[email] = state.user; localStorage.setItem('pw_users', JSON.stringify(users)); }
  localStorage.setItem('pw_current', email); updateUI(); alert(`Welcome back, ${state.user.name}! 🎉`);
  navigateTo(state.selectedFlight ? 'seat' : 'home');
}

function handleRegister(e) {
  e.preventDefault(); const name = document.getElementById('regName').value.trim(); const email = document.getElementById('regEmail').value.trim();
  const phone = document.getElementById('regPhone').value.trim(); const password = document.getElementById('regPassword').value;
  let valid = true; const nameGroup = document.getElementById('regName').parentElement; const emailGroup = document.getElementById('regEmail').parentElement;
  const phoneGroup = document.getElementById('regPhone').parentElement; const passGroup = document.getElementById('regPassword').parentElement;
  if (!name) { nameGroup.classList.add('error'); valid = false; } else nameGroup.classList.remove('error');
  if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) { emailGroup.classList.add('error'); valid = false; } else emailGroup.classList.remove('error');
  if (!/^\+?[\d\s-]{8,}$/.test(phone)) { phoneGroup.classList.add('error'); valid = false; } else phoneGroup.classList.remove('error');
  if (password.length < 6) { passGroup.classList.add('error'); valid = false; } else passGroup.classList.remove('error');
  if (!valid) return;
  const users = JSON.parse(localStorage.getItem('pw_users') || '{}');
  if (users[email]) { alert('Email already registered. Please login.'); return; }
  state.user = { email, name, phone, points: 100, tickets: [] }; users[email] = state.user;
  localStorage.setItem('pw_users', JSON.stringify(users)); localStorage.setItem('pw_current', email);
  updateUI(); alert(`Welcome to PharaohWings, ${name}! 🎉\nYou've earned 100 welcome points.`);
  navigateTo(state.selectedFlight ? 'seat' : 'home');
}

function logout() { state.user = null; localStorage.removeItem('pw_current'); updateUI(); navigateTo('home'); }
function loadUser() { const currentEmail = localStorage.getItem('pw_current'); if (currentEmail) { const users = JSON.parse(localStorage.getItem('pw_users') || '{}'); if (users[currentEmail]) state.user = users[currentEmail]; } }
function saveUser() { if (!state.user) return; const users = JSON.parse(localStorage.getItem('pw_users') || '{}'); users[state.user.email] = state.user; localStorage.setItem('pw_users', JSON.stringify(users)); }
function updateUI() { const pointsEl = document.getElementById('userPoints'); const initialEl = document.getElementById('profileInitial'); if (pointsEl) pointsEl.textContent = state.user ? state.user.points : '0'; if (initialEl) initialEl.textContent = state.user ? state.user.name.charAt(0).toUpperCase() : '?'; }

// ============ HELPERS ============
function formatDate(dateStr) { if (!dateStr) return ''; const d = new Date(dateStr); return d.toLocaleDateString('en-US', { weekday: 'short', month: 'short', day: 'numeric', year: 'numeric' }); }
function addMinutes(time, mins) { const [h, m] = time.split(':').map(Number); const total = h * 60 + m + mins; const nh = Math.floor(total / 60) % 24; const nm = total % 60; return `${String(nh).padStart(2, '0')}:${String(nm).padStart(2, '0')}`; }

// ============ MY BOOKINGS PAGE ============
function renderMyBookings() {
  const container = document.getElementById('bookingsContainer');
  if (!container) return;
  if (!state.user) {
    container.innerHTML = `<div style="text-align:center; padding: 4rem 1rem;"><div style="font-size:4rem; margin-bottom:1rem;">🔒</div><h3 style="margin-bottom:0.5rem;">Please Login to View Your Bookings</h3><p style="color:var(--muted); margin-bottom:1.5rem;">You need an account to access your booking history.</p><button class="btn btn-primary" onclick="navigateTo('login')" style="margin-top:1rem;">Login Now</button></div>`;
    return;
  }
  const allTickets = state.user.tickets || [];
  if (allTickets.length === 0) {
    container.innerHTML = `<div style="text-align:center; padding: 4rem 1rem;"><div style="font-size:4rem; margin-bottom:1rem;">🎫</div><h3 style="margin-bottom:0.5rem;">No Bookings Yet</h3><p style="color:var(--muted); margin-bottom:1.5rem;">Start exploring Egypt and book your first flight!</p><button class="btn btn-primary" onclick="navigateTo('booking')" style="margin-top:1rem;">Book a Flight</button></div>`;
    return;
  }
  const bookingsMap = {};
  allTickets.forEach(ticket => {
    if (!bookingsMap[ticket.bookingRef]) bookingsMap[ticket.bookingRef] = [];
    bookingsMap[ticket.bookingRef].push(ticket);
  });
  let html = `<h3 style="margin-bottom: 1.5rem; font-family: var(--font-head);">Your Bookings</h3><div class="bookings-list" style="display: flex; flex-direction: column; gap: 1.5rem;">`;
  Object.keys(bookingsMap).forEach(ref => {
    const tickets = bookingsMap[ref]; const firstTicket = tickets[0];
    const totalPoints = tickets.reduce((sum, t) => sum + t.points, 0); const paxCount = tickets.length;
    html += `
      <div class="booking-card" style="background: var(--white); border-radius: var(--radius); box-shadow: var(--shadow-sm); overflow: hidden; border: 1px solid var(--sand-2);">
        <div style="background: var(--sand); padding: 1.5rem; display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap; gap: 1rem;">
          <div>
            <div style="font-family: var(--font-head); font-weight: 700; font-size: 1.2rem; color: var(--navy);">Booking Ref: ${ref}</div>
            <div style="color: var(--slate); margin-top: 0.25rem;">${firstTicket.route} • ${formatDate(firstTicket.date)} • ${paxCount} Passenger(s)</div>
          </div>
          <div style="text-align: right;">
            <div style="font-weight: 700; color: var(--gold);">+${totalPoints} pts earned</div>
            <button class="btn btn-primary" style="padding: 0.5rem 1rem; font-size: 0.85rem; margin-top: 0.5rem;" onclick="toggleBookingDetails('${ref}')">View Tickets Details ▼</button>
          </div>
        </div>
        <div id="details-${ref}" style="display: none; padding: 1.5rem; border-top: 1px solid var(--sand-2); background: var(--sand);">
          <h4 style="margin-bottom: 1rem; color: var(--navy);">Passengers & Details</h4>
          <div style="display: flex; flex-direction: column; gap: 1rem;">
            ${tickets.map(t => `
              <div style="background: var(--white); padding: 1.25rem; border-radius: var(--radius-sm); border-left: 4px solid var(--gold);">
                <div style="display: flex; justify-content: space-between; margin-bottom: 0.75rem;">
                  <span style="font-weight: 700; color: var(--navy); font-size: 1.1rem;">${t.passenger.firstName} ${t.passenger.lastName}</span>
                  <span style="background: var(--gold); color: var(--navy); padding: 0.2rem 0.6rem; border-radius: 4px; font-size: 0.8rem; font-weight: 600;">Seat: ${t.seat}</span>
                </div>
                <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 0.75rem; font-size: 0.9rem; color: var(--slate);">
                  <div><strong>DOB:</strong> ${t.passenger.dob}</div>
                  <div><strong>Gender:</strong> ${t.passenger.gender}</div>
                  <div><strong>Nationality:</strong> ${t.passenger.nationality}</div>
                  <div><strong>Passport:</strong> ${t.passenger.passportNo} (Exp: ${t.passenger.passportExpiry})</div>
                  <div><strong>Luggage:</strong> ${t.luggage} (+${t.luggagePrice} EGP)</div>
                  <div><strong>Points:</strong> +${t.points} pts</div>
                </div>
              </div>
            `).join('')}
          </div>
          <div style="margin-top: 1.5rem; text-align: right;">
            <button class="btn btn-ghost" style="padding: 0.5rem 1rem; font-size: 0.85rem;" onclick="alert('E-tickets for booking ${ref} have been sent to ${firstTicket.contactEmail}')">📧 Email E-Tickets</button>
          </div>
        </div>
      </div>
    `;
  });
  html += `</div>`; container.innerHTML = html;
}

function toggleBookingDetails(ref) {
  const detailsDiv = document.getElementById(`details-${ref}`);
  if (detailsDiv.style.display === 'none') { detailsDiv.style.display = 'block'; detailsDiv.scrollIntoView({ behavior: 'smooth', block: 'nearest' }); } 
  else { detailsDiv.style.display = 'none'; }
}

// =========================================================
// ACTIVITIES PAGE FUNCTIONS
// =========================================================
function viewActivities(destinationName) {
  sessionStorage.setItem('selectedDestination', destinationName);
  navigateTo('activities');
}

function renderActivitiesPage() {
  const container = document.getElementById('activitiesContainer');
  if (!container) return;

  const selectedDest = sessionStorage.getItem('selectedDestination');
  let destinationsToShow = DESTINATIONS;
  if (selectedDest) {
    destinationsToShow = DESTINATIONS.filter(d => d.name === selectedDest);
  }

  let html = '';
  destinationsToShow.forEach(dest => {
    if (dest.activities && dest.activities.length > 0) {
      html += `
        <div class="destination-activities-section" style="margin-bottom: 3rem;">
          <div style="display:flex; justify-content:space-between; align-items:center; margin-bottom: 1rem;">
            <h2 style="font-family: var(--font-head); font-size: 2rem; color: var(--navy);">${dest.name} Activities</h2>
            ${selectedDest ? `<button class="btn btn-ghost" onclick="sessionStorage.removeItem('selectedDestination'); renderActivitiesPage();">← View All Destinations</button>` : ''}
          </div>
          <p style="color: var(--slate); margin-bottom: 2rem;">${dest.desc}</p>
          
          <div class="activities-grid" style="display: grid; grid-template-columns: repeat(auto-fit, minmax(300px, 1fr)); gap: 1.5rem;">
            ${dest.activities.map((activity) => `
              <div class="activity-card" style="background: var(--white); border-radius: var(--radius); overflow: hidden; box-shadow: var(--shadow-md); transition: var(--transition); border: 1px solid var(--sand-2);">
                <div class="activity-image" style="height: 200px; background: url('${activity.image}') center/cover no-repeat; position: relative;">
                  <div style="position: absolute; top: 10px; right: 10px; background: rgba(255,255,255,0.95); padding: 5px 10px; border-radius: 20px; font-size: 1.5rem; box-shadow: 0 2px 8px rgba(0,0,0,0.1);">${activity.icon}</div>
                </div>
                <div class="activity-content" style="padding: 1.5rem;">
                  <h3 style="font-family: var(--font-head); font-size: 1.25rem; margin-bottom: 0.75rem; color: var(--navy);">${activity.name}</h3>
                  <div style="display: flex; gap: 1rem; margin-bottom: 1rem; font-size: 0.9rem; color: var(--slate);">
                    <span>⏱ ${activity.duration}</span>
                    <span>💰 ${activity.price}</span>
                  </div>
                  <p style="color: var(--muted); font-size: 0.9rem; margin-bottom: 1.5rem; line-height: 1.5;">${activity.description}</p>
                  <button class="btn btn-primary" style="width: 100%;" onclick="bookActivity('${dest.name}', '${activity.name}')">Book This Activity</button>
                </div>
              </div>
            `).join('')}
          </div>
        </div>
      `;
    }
  });

  if (html === '') {
    html = `<div style="text-align:center; padding: 3rem;"><h3>No activities found for this destination.</h3></div>`;
  }
  container.innerHTML = html;
}

function bookActivity(destination, activity) {
  alert(`Booking ${activity} in ${destination}\n\nRedirecting to flight booking...`);
  state.search.to = destination;
  state.search.from = 'Cairo';
  saveState();
  navigateTo('booking');
}