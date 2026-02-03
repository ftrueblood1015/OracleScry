-- Seed data for CardPurposes table
-- Patterns are pipe-delimited regex patterns (case-insensitive)

-- Clear existing data (optional - comment out if you want to preserve existing)
-- DELETE FROM CardCardPurposes;
-- DELETE FROM CardPurposes;

-- ============ REMOVAL ============
INSERT INTO CardPurposes (Id, Name, Slug, Description, Category, DisplayOrder, IsActive, Patterns, RequiredTypes, ExcludedTypes, ImportedOn, LastUpdatedOn)
VALUES
(NEWID(), 'Creature Destruction', 'creature-destruction', 'Destroys target creatures', 'Removal', 1, 1,
'destroy target creature|destroy target nonland permanent|destroys? all creatures|destroy each creature|destroy target artifact or creature',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Creature Exile', 'creature-exile', 'Exiles target creatures', 'Removal', 2, 1,
'exile target creature|exile target nonland permanent|exile all creatures|exile each creature|exile target attacking creature',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Damage-Based Removal', 'damage-removal', 'Deals damage to creatures', 'Removal', 3, 1,
'deals? \d+ damage to target creature|deals? \d+ damage to any target|deals? \d+ damage to each creature|deals? X damage to target creature',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Artifact/Enchantment Removal', 'artifact-enchantment-removal', 'Removes artifacts or enchantments', 'Removal', 4, 1,
'destroy target artifact|destroy target enchantment|exile target artifact|exile target enchantment|destroy all artifacts|destroy all enchantments',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Bounce', 'bounce', 'Returns permanents to hand', 'Removal', 5, 1,
'return target creature to its owner''s hand|return target nonland permanent to its owner''s hand|return target permanent to its owner''s hand',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Sacrifice Effect', 'sacrifice-effect', 'Forces opponents to sacrifice', 'Removal', 6, 1,
'target player sacrifices a creature|each opponent sacrifices a creature|sacrifice a creature|target opponent sacrifices',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), '-X/-X Effect', 'minus-x-effect', 'Gives creatures -X/-X', 'Removal', 7, 1,
'gets? -\d+/-\d+ until end of turn|gets? -X/-X|all creatures get -\d+/-\d+|-\d+/-\d+ until end of turn',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

-- ============ CARD ADVANTAGE ============
(NEWID(), 'Card Draw', 'card-draw', 'Draws cards', 'CardAdvantage', 10, 1,
'draw a card|draw two cards|draw three cards|draw cards equal to|draw X cards|draws? a card',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Cantrip', 'cantrip', 'Spells that replace themselves', 'CardAdvantage', 11, 1,
'then draw a card|, draw a card|draw a card\.$',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Card Selection', 'card-selection', 'Scry, surveil, or look at cards', 'CardAdvantage', 12, 1,
'scry \d+|surveil \d+|look at the top \d+ cards|look at the top card',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Tutor', 'tutor', 'Searches library for specific cards', 'CardAdvantage', 13, 1,
'search your library for a card|search your library for an? \w+ card|search your library for a creature|search your library for an instant',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

-- ============ RAMP ============
(NEWID(), 'Mana Dork', 'mana-dork', 'Creatures that produce mana', 'Ramp', 20, 1,
'add \{[WUBRGC]\}|add one mana of any color|add \d+ mana|tap: add',
'Creature', 'Land', GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Land Ramp', 'land-ramp', 'Puts lands onto the battlefield', 'Ramp', 21, 1,
'search your library for a basic land card and put it onto the battlefield|search your library for a land card and put it onto the battlefield|put a land card from your hand onto the battlefield|land onto the battlefield tapped',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Mana Rock', 'mana-rock', 'Artifacts that produce mana', 'Ramp', 22, 1,
'tap: add \{[WUBRGC]\}|tap: add one mana|tap: add \d+ mana',
'Artifact', 'Land|Creature', GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Cost Reduction', 'cost-reduction', 'Reduces spell costs', 'Ramp', 23, 1,
'spells? you cast costs? \{?\d+\}? less|costs? \{\d+\} less to cast|affinity for',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

-- ============ COUNTERSPELL ============
(NEWID(), 'Hard Counter', 'hard-counter', 'Counters spells unconditionally', 'Counterspell', 30, 1,
'counter target spell(?! unless)|counter that spell(?! unless)',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Soft Counter', 'soft-counter', 'Counters spells conditionally', 'Counterspell', 31, 1,
'counter target spell unless|counter that spell unless its controller pays',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Creature Counter', 'creature-counter', 'Counters creature spells', 'Counterspell', 32, 1,
'counter target creature spell',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Noncreature Counter', 'noncreature-counter', 'Counters noncreature spells', 'Counterspell', 33, 1,
'counter target noncreature spell|counter target instant or sorcery',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

-- ============ COMBAT ============
(NEWID(), 'Pump Spell', 'pump-spell', 'Gives +X/+X temporarily', 'Combat', 40, 1,
'gets? \+\d+/\+\d+ until end of turn|get \+\d+/\+\d+ until end of turn|target creature gets \+',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Evasion', 'evasion', 'Grants flying, unblockable, etc.', 'Combat', 41, 1,
'can''t be blocked|flying|menace|trample|shadow|fear|intimidate|skulk',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'First Strike/Double Strike', 'first-strike', 'Grants first or double strike', 'Combat', 42, 1,
'first strike|double strike',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Vigilance', 'vigilance', 'Grants vigilance', 'Combat', 43, 1,
'vigilance',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Haste', 'haste', 'Grants haste', 'Combat', 44, 1,
'haste|can attack as though it had haste',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

-- ============ PROTECTION ============
(NEWID(), 'Hexproof', 'hexproof', 'Grants hexproof', 'Protection', 50, 1,
'hexproof|can''t be the target of spells or abilities your opponents control',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Indestructible', 'indestructible', 'Grants indestructible', 'Protection', 51, 1,
'indestructible',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Protection From', 'protection-from', 'Grants protection from colors/types', 'Protection', 52, 1,
'protection from \w+|has protection from',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Regenerate', 'regenerate', 'Regeneration effects', 'Protection', 53, 1,
'regenerate|regenerates',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Ward', 'ward', 'Grants ward', 'Protection', 54, 1,
'ward \{|ward—',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

-- ============ RECURSION ============
(NEWID(), 'Graveyard Return', 'graveyard-return', 'Returns cards from graveyard to hand/battlefield', 'Recursion', 60, 1,
'return target \w+ card from your graveyard to your hand|return target \w+ card from your graveyard to the battlefield|return target creature card from your graveyard',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Reanimate', 'reanimate', 'Puts creatures from graveyard onto battlefield', 'Recursion', 61, 1,
'put target creature card from a graveyard onto the battlefield|return target creature card from your graveyard to the battlefield|put a creature card from your graveyard onto the battlefield',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Flashback', 'flashback', 'Can be cast from graveyard', 'Recursion', 62, 1,
'flashback|you may cast this card from your graveyard',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

-- ============ TOKENS ============
(NEWID(), 'Token Creation', 'token-creation', 'Creates creature tokens', 'Tokens', 70, 1,
'create a \d+/\d+ \w+ creature token|create \d+ \d+/\d+ \w+ creature tokens|creates? a token|creates? \d+ tokens|create a creature token',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Token Doubling', 'token-doubling', 'Doubles or multiplies tokens', 'Tokens', 71, 1,
'create twice that many|double the number of|if you would create a token, instead create',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

-- ============ LIFE GAIN ============
(NEWID(), 'Life Gain', 'life-gain', 'Gains life', 'LifeGain', 80, 1,
'gain \d+ life|gains? life equal to|you gain life|gains? \d+ life|lifelink',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Lifegain Payoff', 'lifegain-payoff', 'Benefits from gaining life', 'LifeGain', 81, 1,
'whenever you gain life|if you gained life this turn|whenever you gain or lose life',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

-- ============ DAMAGE ============
(NEWID(), 'Direct Damage', 'direct-damage', 'Deals damage to players', 'Damage', 90, 1,
'deals? \d+ damage to target player|deals? \d+ damage to target opponent|deals? \d+ damage to each opponent|deals? X damage to target player',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Burn', 'burn', 'Flexible damage to any target', 'Damage', 91, 1,
'deals? \d+ damage to any target|deals? X damage to any target|deals? \d+ damage divided as you choose',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

-- ============ MILL ============
(NEWID(), 'Mill', 'mill', 'Puts cards from library into graveyard', 'Mill', 100, 1,
'mills? \d+ cards|put the top \d+ cards of .+ library into .+ graveyard|target player mills|each opponent mills',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Self-Mill', 'self-mill', 'Mills your own library', 'Mill', 101, 1,
'put the top \d+ cards of your library into your graveyard|mill \d+ cards',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

-- ============ DISCARD ============
(NEWID(), 'Targeted Discard', 'targeted-discard', 'Forces opponent to discard specific cards', 'Discard', 110, 1,
'target opponent reveals their hand.+choose|look at target opponent''s hand.+choose|target player discards a card',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Mass Discard', 'mass-discard', 'Forces multiple discards', 'Discard', 111, 1,
'each opponent discards|target opponent discards \d+ cards|discards? their hand|discard your hand',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

-- ============ OTHER ============
(NEWID(), 'Equipment', 'equipment', 'Equipment cards', 'Other', 120, 1,
'equip \{|equipped creature|equip—',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Aura', 'aura', 'Aura enchantments', 'Other', 121, 1,
'enchant creature|enchant permanent|enchanted creature',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Enters the Battlefield', 'etb', 'ETB triggers', 'Other', 122, 1,
'when .+ enters the battlefield|enters the battlefield,|whenever a creature enters the battlefield under your control',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Death Trigger', 'death-trigger', 'Dies triggers', 'Other', 123, 1,
'when .+ dies|whenever a creature dies|whenever .+ is put into a graveyard from the battlefield',
NULL, NULL, GETUTCDATE(), GETUTCDATE()),

(NEWID(), 'Deathtouch', 'deathtouch', 'Has or grants deathtouch', 'Other', 124, 1,
'deathtouch',
NULL, NULL, GETUTCDATE(), GETUTCDATE());

-- Verify the insert
SELECT Category, COUNT(*) as Count FROM CardPurposes GROUP BY Category ORDER BY Category;
