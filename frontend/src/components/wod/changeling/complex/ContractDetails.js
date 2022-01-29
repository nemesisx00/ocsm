import './ContractDetails.css'
import React from 'react'
import {normalizeClassNames} from '../../../core/Utilities'

const EmptyClause = {
	label: '',
	dots: 0,
	description: '',
	cost: '',
	dicePool: '',
	action: '',
	catch: '',
	resultDramatic: '',
	resultFailure: '',
	resultSuccess: '',
	resultExceptional: '',
	modifiers: []
}

export default class ContractDetails extends React.Component
{
	constructor(props)
	{
		super(props)
		
		this.state = {
			currentTab: 0,
			editMode: false,
			newModifier: false
		}
	}
	
	render()
	{
		let tabs = []
		let clauses = []
		
		if(this.props.contract !== null)
		{
			this.props.contract.clauses.forEach((clause, i) => {
				let tab = (
					<div className={normalizeClassNames('clause tab', this.state.currentTab === i ? 'selected' : '')} onClick={() => this.changeTab(i)} key={i}>
						{`${clause.label} (${clause.dots})`}
					</div>
				)
				tabs.push(tab)
				
				clauses.push(
					this.state.editMode
						? this.generateEditView(clause, i)
						: this.generateDisplayView(clause, i)
				)
			})
		}
		
		if(clauses.length < this.props.contract.dots)
		{
			let max = this.props.contract.dots - clauses.length
			for(let i = 0; i < max; i++)
			{
				let tab = (
					<div className={normalizeClassNames('clause tab', this.state.currentTab === i ? 'selected' : '')} onClick={() => this.changeTab(i)} key={i}>
						New Clause
					</div>
				)
				tabs.push(tab)
				
				let newClause = Object.assign({}, EmptyClause)
				clauses.push(
					this.state.editMode
						? this.generateEditView(newClause, i)
						: this.generateDisplayView(newClause, i)
				)
			}
		}
		
		return (
			<div className={normalizeClassNames('contractDetails fullscreenOverlayItem', this.props.className)}>
				<div className="tabs">
					{tabs}
				</div>
				<div className="clauses">
					{clauses}
				</div>
			</div>
		)
	}
	
	generateDisplayView(clause, key)
	{
		let {mods, sits} = this.generateModifierLists(clause)
		//<DotsGroup value={clause.dots} valueChangedHandler={() => {}} />
		return (
			<div className={normalizeClassNames('contractDetails', this.props.className, this.state.currentTab === key ? 'selected' : '')} key={key}>
				<div className="row">
					{clause.label}
					
					<button onClick={() => this.toggleEditMode()}>Edit</button>
				</div>
				<hr />
				<div className="row">{clause.description}</div>
				<hr />
				<div className="row indent">Cost: {clause.cost}</div>
				<div className="row indent">Dice Pool: {clause.dicePool}</div>
				<div className="row indent">Action: {clause.action}</div>
				<div className="row indent">Catch: {clause.catch}</div>
				<hr />
				<div className="column">
					<div className="row heading">Roll Results</div>
					<div className="row indent">Dramatic Failure: {clause.resultDramatic}</div>
					<hr />
					<div className="row indent">Failure: {clause.resultFailure}</div>
					<hr />
					<div className="row indent">Success: {clause.resultSuccess}</div>
					<hr />
					<div className="row indent">Exceptional Success: {clause.resultExceptional}</div>
				</div>
				<hr />
				<div className="row">Suggested Modifiers</div>
				<hr />
				<div className="row indent">
					<div className="column">
						<div>Modifier</div>
						{mods}
					</div>
					<div className="column">
						<div>Situation</div>
						{sits}
					</div>
				</div>
			</div>
		)
	}
	
	generateEditView(clause, key)
	{
		let mods = this.generateModifierLists(clause)
		//<DotsGroup value={clause.dots} valueChangedHandler={(arg) => this.props.changeHandler(this.props.contract, clause, 'dots', arg)} />
		return (
			<div className={`contractDetails ${this.props.className}`} key={key}>
				<div className="row">
					<input type="text" value={clause.label} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'label', e.target.value)} />
					
					<button onClick={() => this.toggleEditMode()}>Done</button>
				</div>
				<hr />
				<div className="row">
					<textarea value={clause.description} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'description', e.target.value)} />
				</div>
				<hr />
				<div className="row indent spacer">
					Cost:
					<input type="text" value={clause.cost} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'cost', e.target.value)} />
				</div>
				<div className="row indent spacer">
					Dice Pool:
					<input type="text" value={clause.dicePool} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'dicePool', e.target.value)} />
				</div>
				<div className="row indent spacer">
					Action:
					<input type="text" value={clause.action} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'action', e.target.value)} />
				</div>
				<div className="column indent">
					Catch:
					<textarea value={clause.catch} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'catch', e.target.value)} />
				</div>
				<hr />
				<div className="column">
					<div className="row heading">Roll Results</div>
					<div className="column indent">
						Dramatic Failure:
						<textarea value={clause.resultDramatic} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'resultDramatic', e.target.value)} />
					</div>
					<hr />
					<div className="column indent">
						Failure:
						<textarea value={clause.resultFailure} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'resultFailure', e.target.value)} />
					</div>
					<hr />
					<div className="column indent">
						Success:
						<textarea value={clause.resultSuccess} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'resultSuccess', e.target.value)} />
					</div>
					<hr />
					<div className="column indent">
						Exceptional Success:
						<textarea value={clause.resultExceptional} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'resultExceptional', e.target.value)} />
					</div>
				</div>
				<hr />
				<div className="row">
					<div className="label">Suggested Modifiers</div>
					{typeof(clause) !== 'undefined' && clause !== null && clause.label !== '' && clause.dots > 0 &&
						<button onClick={() => this.props.changeHandler(this.props.contract, clause, 'modifier', '', clause.modifiers.length)}>Add New Modifier</button>}
				</div>
				<hr />
				<div className="column indent modifiers">
					<div className="row label">
						<div className="small">Modifier</div>
						<div>Situation</div>
					</div>
					{mods}
				</div>
			</div>
		)
	}
	
	generateModifierLists(clause)
	{
		let out = null
		let mode = this.state.editMode
		
		if(mode)
		{
			out = []
			clause.modifiers.forEach((obj, i) => {
				out.push(
					(<div className="row" key={`modifierRow-${i}`}>
						<input type="text" className="small spacer" value={obj.modifier} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'modifier', e.target.value, i)} />
						<textarea className="spacer" value={obj.situation} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'situation', e.target.value, i)} />
					</div>)
				)
			})
		}
		else
		{
			out = {
				mods: [],
				sits: []
			}
			
			let mod = null
			let sit = null
			clause.modifiers.forEach((obj, i) => {
				//Make sure there's at least a space inside each to ensure the layout flows correctly.
				mod = obj.modifier
				if(mod === '')
					mod = ' '
				
				sit = obj.situation
				if(sit === '')
					sit = ' '
				
				out.mods.push(
					(<div key={`modifier-${i}`}>{mod}</div>)
				)
				out.sits.push(
					(<div key={`situation-${i}`}>{sit}</div>)
				)
			})
		}
		
		return out
	}
	
	changeTab(tabIndex)
	{
		let newState = {...this.state}
		newState.currentTab = tabIndex
		this.setState(() => { return newState })
	}
	
	toggleEditMode()
	{
		let newState = {...this.state}
		newState.editMode = !newState.editMode
		this.setState(() => { return newState })
	}
}
