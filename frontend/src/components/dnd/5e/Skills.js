import './Skills.css'
import React from 'react'
import AbilityScores from './AbilityScores'
import { compareStrings, normalizeClassNames } from '../../core/Utilities'

const StrengthSkills = Object.freeze({
	Athletics: 'athletics'
})

const DexteritySkills = Object.freeze({
	Acrobatics: 'acrobatics',
	'Sleight of Hand': 'sleightOfHand',
	Stealth: 'stealth'
})

const IntelligenceSkills = Object.freeze({
	Arcana: 'arcana',
	History: 'history',
	Investigation: 'investigation',
	Nature: 'nature',
	Religion: 'religion'
})

const WisdomSkills = Object.freeze({
	'Animal Handling': 'animalHandling',
	Insight: 'insight',
	Medicine: 'medicine',
	Perception: 'perception',
	Survival: 'survival'
})

const CharismaSkills = Object.freeze({
	Deception: 'deception',
	Intimidation: 'intimidation',
	Performance: 'performance',
	Persuasion: 'persuasion'
})

const ProficiencyType = Object.freeze({
	Not: 'not',
	Half: 'half',
	Proficient: 'proficient',
	Expert: 'expert'
})

const CalculateSkillModifier = (abilityScore, bonus, proficiency) => {
	let mod = AbilityScores.CalculateAbilityModifier(abilityScore)
	
	switch(proficiency)
	{
		case ProficiencyType.Proficient:
			mod += bonus
			break
		case ProficiencyType.Expert:
			mod += bonus * 2
			break
		case ProficiencyType.Half:
			mod += Math.trunc(bonus / 2)
			break
		default:
			break
	}
	
	return mod
}

class Skill extends React.Component
{
	render()
	{
		let proficiencyClass = ''
		switch(this.props.proficiency)
		{
			case ProficiencyType.Proficient:
				proficiencyClass = ProficiencyType.Proficient
				break
			case ProficiencyType.Expert:
				proficiencyClass = ProficiencyType.Expert
				break
			case ProficiencyType.Half:
				proficiencyClass = ProficiencyType.Half
				break
			default:
				break
		}
		
		return (
			<div className={normalizeClassNames('skill row', this.props.className)}>
				<div className={normalizeClassNames('proficiency', proficiencyClass)} onClick={(e) => this.props.changeHandler()}>
					<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 12 12">
						<circle cx="6" cy="6" r="6" />
						<path d="M6,0 Q20,6 6,12 Z" />
					</svg>
				</div>
				<div className="label">{this.props.label}</div>
				<div className="value">{CalculateSkillModifier(this.props.value, this.props.proficiencyBonus, this.props.proficiency)}</div>
			</div>
		)
	}
}

class Skills extends React.Component
{
	render()
	{
		let skillLists = this.generateSkillLists()
		
		return (
			<div className={normalizeClassNames('skillsWrapper column', this.props.className)}>
				<div className="skillsLabel">Skills</div>
				<div className="skills row">
					<div className="column">
						<div className="skillsLabel">Strength</div>
						{skillLists.Strength}
					</div>
					<div className="column">
						<div className="skillsLabel">Dexterity</div>
						{skillLists.Dexterity}
					</div>
					<div className="column">
						<div className="skillsLabel">Intelligence</div>
						{skillLists.Intelligence}
					</div>
					<div className="column">
						<div className="skillsLabel">Wisdom</div>
						{skillLists.Wisdom}
					</div>
					<div className="column">
						<div className="skillsLabel">Charisma</div>
						{skillLists.Charisma}
					</div>
				</div>
			</div>
		)
	}
	
	generateSkillLists()
	{
		let skillLists = {
			Strength: [],
			Dexterity: [],
			Intelligence: [],
			Wisdom: [],
			Charisma: []
		}
		
		console.log(this.props.abilityScores)
		
		Object.entries(StrengthSkills).forEach((entry, i) => {
			let [upper, lower] = entry
			skillLists.Strength.push(
				(<Skill key={`strength-${i}`} label={upper} proficiencyBonus={this.props.proficiencyBonus} proficiency={this.props.skills[lower]} value={this.props.abilityScores.strength} changeHandler={() => this.props.changeHandler(lower)} />)
			)
		})
		
		Object.entries(DexteritySkills).forEach((entry, i) => {
			let [upper, lower] = entry
			skillLists.Dexterity.push((<Skill key={`dexterity-${i}`} label={upper} proficiencyBonus={this.props.proficiencyBonus} proficiency={this.props.skills[lower]} value={this.props.abilityScores.dexterity} changeHandler={() => this.props.changeHandler(lower)} />))
		})
		
		Object.entries(IntelligenceSkills).forEach((entry, i) => {
			let [upper, lower] = entry
			skillLists.Intelligence.push((<Skill key={`intelligence-${i}`} label={upper} proficiencyBonus={this.props.proficiencyBonus} proficiency={this.props.skills[lower]} value={this.props.abilityScores.intelligence} changeHandler={() => this.props.changeHandler(lower)} />))
		})
		
		Object.entries(WisdomSkills).forEach((entry, i) => {
			let [upper, lower] = entry
			skillLists.Wisdom.push((<Skill key={`wisdom-${i}`} label={upper} proficiencyBonus={this.props.proficiencyBonus} proficiency={this.props.skills[lower]} value={this.props.abilityScores.wisdom} changeHandler={() => this.props.changeHandler(lower)} />))
		})
		
		Object.entries(CharismaSkills).forEach((entry, i) => {
			let [upper, lower] = entry
			skillLists.Charisma.push((<Skill key={`charisma-${i}`} label={upper} proficiencyBonus={this.props.proficiencyBonus} proficiency={this.props.skills[lower]} value={this.props.abilityScores.charisma} changeHandler={() => this.props.changeHandler(lower)} />))
		})
		
		return skillLists
	}
}

let allSkills = Object.assign({}, StrengthSkills, DexteritySkills, IntelligenceSkills, WisdomSkills, CharismaSkills)
Object.entries(allSkills).sort((a, b) => compareStrings(a.value, b.value))

Skills.All = Object.freeze(allSkills)
Skills.Strength = StrengthSkills
Skills.Dexterity = DexteritySkills
Skills.Intelligence = IntelligenceSkills
Skills.Wisdom = WisdomSkills
Skills.Charisma = CharismaSkills

Skills.Proficiency = ProficiencyType

export default Skills
